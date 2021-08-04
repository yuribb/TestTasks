using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Controllers
{
    internal interface IRedisClient
    {
        int Get(string type);
        void Set(string type, int current);
    }

    public static class FiguresStorage
    {
        // корректно сконфигурированный и готовый к использованию клиент Редиса
        private static IRedisClient RedisClient { get; }

        public static bool CheckIfAvailable(string type, int count)
        {
            return RedisClient.Get(type) >= count;
        }

        public static void Reserve(string type, int count)
        {
            var current = RedisClient.Get(type);

            RedisClient.Set(type, current - count);
        }
    }

    //Свойства сущностей, которые выходят наружу лучше сделать неизменяемыми после инициализации.
    public class Position
    {
        public string Type { get; set; } //лучше использовать enum.
        //Лучше использовать DTO объект и мапить его, либо массив для сторон, если объекты не хотим создавать.
        //В исправленном оставлю как есть но так не делается, т.к. мы опять используем вместро абстракции реализацию, что осложняет масштабирование
        public float SideA { get; set; } //разный тип данных у сущностей которые взаимодействуют между собой. Можно потерять в точности.
        public float SideB { get; set; } 
        public float SideC { get; set; }

        public int Count { get; set; }
    }

    public class Cart
    {
        public List<Position> Positions { get; set; }
    }

    public class Order
    {
        public List<Figure> Positions { get; set; }

        //Метод нигде не используется, он не нужен. Оставлю его как есть, но технически нам он не нужен, как не нужен метод GetArea().
        public decimal GetTotal() =>
            Positions.Select(p => p switch
                {
                    Triangle => (decimal)p.GetArea() * 1.2m, //Надо привести все числовые типы данных к decimal, т.к. он лучше подходит для хранения и отображения чисел.
                    Circle => (decimal)p.GetArea() * 0.9m
                    //нет квадрата
                    //нет дефолтной обработки
                })
                .Sum();
    }

    public abstract class Figure //Абстрактный класс содержит не абстрактные данные.
    {
        public float SideA { get; set; }
        public float SideB { get; set; }
        public float SideC { get; set; }

        public abstract void Validate();
        public abstract double GetArea(); //Можно заменить на свойство
    }

    public class Triangle : Figure //Для классов нет конструкторов.
    {
        public override void Validate() //Валидацию также можно перенести в инициализацию. Это сразу поможет избежать создание некорректных фигур, но это не обязательно, т.к. возможно должна быть их поддержка для логгирования.
        {
            bool CheckTriangleInequality(float a, float b, float c) => a < b + c; //Название метода лучше поменять на IsTriangleInequality
            if (CheckTriangleInequality(SideA, SideB, SideC)
                && CheckTriangleInequality(SideB, SideA, SideC)
                && CheckTriangleInequality(SideC, SideB, SideA))
                return;
            throw new InvalidOperationException("Triangle restrictions not met");
        }

        public override double GetArea()
        {
            var p = (SideA + SideB + SideC) / 2;
            return Math.Sqrt(p * (p - SideA) * (p - SideB) * (p - SideC));
        }

    }

    public class Square : Figure
    {
        public override void Validate()
        {
            if (SideA < 0) //Оставить один if с ||
                throw new InvalidOperationException("Square restrictions not met"); //Можно использовать nameof, тогда при смене имени текст ошибки не сломается

            if (SideA != SideB)
                throw new InvalidOperationException("Square restrictions not met");
        }

        public override double GetArea() => SideA * SideA; //заменить на свойство
    }

    public class Circle : Figure
    {
        public override void Validate()
        {
            if (SideA < 0)
                throw new InvalidOperationException("Circle restrictions not met");
        }

        public override double GetArea() => Math.PI * SideA * SideA; //заменить на свойство
    }

    public interface IOrderStorage
    {
        // сохраняет оформленный заказ и возвращает сумму
        Task<decimal> Save(Order order);
    }

    [ApiController]
    [Route("[controller]")]
    public class FiguresController : ControllerBase
    {
        private readonly ILogger<FiguresController> _logger; //логгирования в контроллере нет, логгер необязателен
        private readonly IOrderStorage _orderStorage;

        public FiguresController(ILogger<FiguresController> logger, IOrderStorage orderStorage)
        {
            _logger = logger;
            _orderStorage = orderStorage;
        }

        // хотим оформить заказ и получить в ответе его стоимость
        [HttpPost] //В метод коньроллера лучше добавить имя, так можно в одном контроллере реализовать несколько POST запросов
        public async Task<ActionResult> Order(Cart cart) //Асинхронного кода нет. Можно обойтись без async/await
        {
            foreach (var position in cart.Positions) //Можно переделать в LINQ Any
            {
                if (!FiguresStorage.CheckIfAvailable(position.Type, position.Count))
                {
                    return new BadRequestResult();
                }
            }

            var order = new Order
            {
                Positions = cart.Positions.Select(p =>
                {
                    Figure figure = p.Type switch
                    {
                        "Circle" => new Circle(), //Добавить конструкторы, инициализировать в них.
                        "Triangle" => new Triangle(),
                        "Square" => new Square()
                        //Добавить обработку по умолчанию
                    };
                    figure.SideA = p.SideA;
                    figure.SideB = p.SideB;
                    figure.SideC = p.SideC;
                    figure.Validate();
                    return figure;
                }).ToList()
            };
            //Логику из контроллера лучше выносить. В переделанном я оставлю, чтобы не городить сервисов. Но в контроллере как правило остаётся только валидация, маппинг и возврат результата
            foreach (var position in cart.Positions)
            {
                FiguresStorage.Reserve(position.Type, position.Count);
            }

            var result = _orderStorage.Save(order);

            return new OkObjectResult(result.Result);
        }
    }
}