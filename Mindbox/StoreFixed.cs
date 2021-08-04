using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TestReview.ApiHost.Controllers.V1
{
    internal interface IRedisClient
    {
        int Get(FigureType type);
        void Set(FigureType type, int current);
    }

    public static class FiguresStorage
    {
        // корректно сконфигурированный и готовый к использованию клиент Редиса
        private static IRedisClient RedisClient { get; }

        public static bool CheckIfAvailable(FigureType type, int count)
        {
            return RedisClient.Get(type) >= count;
        }

        public static void Reserve(FigureType type, int count)
        {
            var current = RedisClient.Get(type);

            RedisClient.Set(type, current - count);
        }
    }

    public enum FigureType
    {
        Circle = 0,
        Triangle = 1,
        Square = 2
    }

    public class Position
    {
        public FigureType Type { get; }
        public decimal SideA { get; }
        public decimal SideB { get; }
        public decimal SideC { get; }
        public int Count { get; }
    }

    public class Cart
    {
        public List<Position> Positions { get; set; }
    }

    public class Order
    {
        public List<Figure> Positions { get; set; }

        public decimal GetTotal() =>
            Positions.Select(p => p switch
            {
                Triangle => p.Area * 1.2m,
                Circle => p.Area * 0.9m,
                Square => p.Area,
                _ => throw new ArgumentException("Unsupported figure")
            })
                .Sum();
    }

    public abstract class Figure 
    {
        public abstract void Validate();
        public abstract decimal Area { get; }
    }

    public class Triangle : Figure
    {
        public decimal SideA { get; }
        public decimal SideB { get; }
        public decimal SideC { get; }

        public override decimal Area
        {
            get
            {
                var p = (SideA + SideB + SideC) / 2;
                var side = p * (p - SideA) * (p - SideB) * (p - SideC);
                return (decimal)Math.Sqrt((double)side); //В данном случае можно сделать так, хоть это и не очень точно, но для нашего случая хватает. Для финансовых операций или научных рассчётов так делать нельзя.
            }
        }

        public Triangle(decimal a, decimal b, decimal c)
        {
            this.SideA = a;
            this.SideB = b;
            this.SideC = c;
        }

        public override void Validate()
        {
            bool IsTriangleInequality(decimal a, decimal b, decimal c) => a < b + c;

            if (IsTriangleInequality(SideA, SideB, SideC)
                && IsTriangleInequality(SideB, SideA, SideC)
                && IsTriangleInequality(SideC, SideB, SideA))
                return;
            throw new InvalidOperationException($"{nameof(Triangle)} restrictions not met");
        }
    }

    public class Square : Figure
    {
        public decimal SideA { get; }
        public decimal SideB { get; }

        public Square(decimal a, decimal b)
        {
            this.SideA = a;
            this.SideB = b;
        }

        public override void Validate()
        {
            if (SideA < 0 || SideA != SideB)
                throw new InvalidOperationException($"{nameof(Square)} restrictions not met");
        }

        public override decimal Area => SideA * SideA;
    }

    public class Circle : Figure
    {
        public decimal SideA { get; }

        public Circle(decimal a)
        {
            this.SideA = a;
        }

        public override void Validate()
        {
            if (SideA < 0)
                throw new InvalidOperationException($"{nameof(Circle)} restrictions not met");
        }

        public override decimal Area => (decimal)(Math.PI * (double)SideA * (double)SideA); //здесь также придётся делать каст
    }

    public interface IOrderStorage
    {
        // сохраняет оформленный заказ и возвращает сумму
        Task<decimal> Save(Order order);
    }

    /// <summary>
    /// controller
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/v{version:apiVersion}/figures")]
    public class FiguresController : ControllerBase
    {
        private readonly ILogger<FiguresController> _logger;
        private readonly IOrderStorage _orderStorage;

        public FiguresController(ILogger<FiguresController> logger, IOrderStorage orderStorage)
        {
            _logger = logger;
            _orderStorage = orderStorage;
        }

        // хотим оформить заказ и получить в ответе его стоимость
        [HttpPost("order")]
        public ActionResult Order([FromBody] Cart cart)
        {
            _logger.LogInformation("Order");

            if (cart.Positions.Any(position => !FiguresStorage.CheckIfAvailable(position.Type, position.Count)))
            {
                return new BadRequestResult();
            }

            var order = new Order
            {
                Positions = cart.Positions.Select(p =>
                {
                    Figure figure = p.Type switch
                    {
                        FigureType.Circle => new Circle(p.SideA),
                        FigureType.Triangle => new Triangle(p.SideA, p.SideB, p.SideC),
                        FigureType.Square => new Square(p.SideA, p.SideB),
                        _ => throw new ArgumentOutOfRangeException("Unsupported figure")
                    };
                    figure.Validate();
                    return figure;
                }).ToList()
            };

            foreach (var position in cart.Positions)
            {
                FiguresStorage.Reserve(position.Type, position.Count);
            }

            var result = _orderStorage.Save(order);

            return new OkObjectResult(result.Result);
        }
    }
}
