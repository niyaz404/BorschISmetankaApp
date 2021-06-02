using Microsoft.VisualStudio.TestTools.UnitTesting;
using BorschISmetanka.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace BorschISmetanka.Views.Tests
{
    [TestClass()]
    public class RegistrationTests
    {
        [TestMethod("email")]
        [DataRow("", false)]
        [DataRow("dsdf", false)]
        [DataRow("g@mail.ru", true)]
        [DataRow("g@gmail@gmail.ru", false)]
        [DataRow("g@gmail,ru", false)]
        [DataRow("mail@gmail.com", true)]
        public void CheckEmailValid_Emails_ReturnValidity(string email, bool expected)
        {
            //arrange

            //actual
            var actual = Registration.CheckEmailValid(email);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod("number")]
        [DataRow("", false)]
        [DataRow("89174734313", true)]
        [DataRow("79174734313", true)]
        [DataRow("88005553535", false)]
        [DataRow("12345678987", false)]
        [DataRow("8917", false)]
        public void CheckNumValid_PhoneNumber_ReturnValidity(string num, bool expected)
        {
            //arrange

            //actual
            var actual = Registration.CheckNumValid(num);

            //assert
            Assert.AreEqual(expected, actual);
        }        
    }
}