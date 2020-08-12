using System;
using System.Collections.Generic;
using System.Text;
using TukiTaki_KenaKata.service.model;

namespace TukiTaki_KenaKata.model
{
    class ProductDTO : Component
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        public ProductDTO()
        {

        }
        public ProductDTO(Guid id, string name, string description, double price)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Price = price;
        }

        public string ToString()
        {

            return $"" +
                $"{Helper.MyOutputString(this.Id.ToString() + "\n", "m")}" +
                $"{Helper.MyOutputString(this.Name + "", "g")} " +
                $"({Helper.MyOutputString(this.Description+ "", "b")}) : " +
                $"{Helper.MyOutputString(((long)this.Price).ToString() + "", "r")}";
        }
        public void Display()
        {
            Console.WriteLine($"" +
                $"{Helper.MyOutputString(this.Id.ToString() + "\n", "m")}" +
                $"{Helper.MyOutputString(this.Name + "", "g")} " +
                $"({Helper.MyOutputString(this.Description + "", "b")}) : " +
                $"{Helper.MyOutputString(((long)this.Price).ToString() + "", "r")}");
        }

        public double GetPrice()
        {
            return this.Price;
        }

        public double GetDiscount()
        {
            return 0;
        }
    }
}
