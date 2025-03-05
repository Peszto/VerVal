using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


[assembly:InternalsVisibleTo("DatesAndStuff.Tests")]

namespace DatesAndStuff
{
    
    internal class Person
    {
        private bool married = false;

        public string Name { get; private set; }

        public double Salary { get; private set; }

        public Person(string name, double salary) {
            this.Name = name;
            this.Salary = salary;
            this.married = false;
        }

        public void GotMarried(string newName)
        {
            if (married)
                throw new Exception("Poligamy not yet supported.");

            this.married = true;
            this.Name = newName;
        }

        public void IncreaseSalary(double percentage)
        {
            if (percentage <= -10 )
                throw new ArgumentOutOfRangeException(nameof(percentage));

            this.Salary = this.Salary * (1 + percentage / 100);
        }

        public static Person Clone(Person p)
        {
            return new Person(p.Name, p.Salary);
        }
    }
}
