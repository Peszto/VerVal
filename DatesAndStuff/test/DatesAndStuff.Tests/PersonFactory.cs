﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatesAndStuff.Tests
{
    internal class PersonFactory
    {
        public static Person CreateTestPerson()
        {
            return CreateTestPerson(fp => { });
        }

        public static Person CreateTestPerson(Action<FoodPreferenceParams> foodPreferenceModifyer)
        {
            var fp = new FoodPreferenceParams()
            {
                CanEatChocolate = true,
                CanEatEgg = true,
                CanEatLactose = true,
                CanEatGluten = true
            };

            foodPreferenceModifyer(fp);

            return new Person("Test Pista",
             new EmploymentInformation(
                 54,
                 new Employer("RO1234567", "Valami city valami hely", "Dagobert bacsi", new List<int>() { 6201, 7210 })),
             new TestPaymentService(600),
             new LocalTaxData("4367558"),
             fp
            );
        }
    }
}
