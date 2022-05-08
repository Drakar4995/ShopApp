using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
//Necesario para obtener Find dentro de las ICollection o IList
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace ShopApp.UIT.RealizarPedido_UIT
{

    public class CURealizarPedido_UIT : IDisposable
    {
        IWebDriver _driver;
        string _URI;
        void IDisposable.Dispose() {
            _driver.Close();
            _driver.Dispose();
            GC.SuppressFinalize(this);
        }
       

        public CURealizarPedido_UIT()
        {
            UtilitiesUIT.SetUp_UIT(out _driver, out _URI);
            Initial_step_opening_the_web_page();
        }
        private void Initial_step_opening_the_web_page()
        {
            _driver.Navigate()
                .GoToUrl(_URI);
        }

        private void Precondition_perform_login()
        {

            _driver.Navigate()
                    .GoToUrl(_URI + "Identity/Account/Login");
            _driver.FindElement(By.Id("Input_Email"))
                .SendKeys("peter@uclm.com");

            _driver.FindElement(By.Id("Input_Password"))
                .SendKeys("OtherPass12$");

            _driver.FindElement(By.Id("login-submit"))
                .Click();
        }

        private void First_step_accessing_purchases()
        {
            _driver.FindElement(By.Id("ComprasController")).Click();
           

        }
        private void Second_step_accessing_link_Create_New()
        {
            _driver.FindElement(By.Id("RealizarCompra")).Click();


        }

        private void Third_filter_prendas_byNombre(string nombreFilter)
        {
            _driver.FindElement(By.Id("prendaNombre")).SendKeys(nombreFilter);

            _driver.FindElement(By.Id("filterbyNombrePrecioMarca")).Click();

        }

        private void Third_filter_prendas_byMarca(string marcaSelected)
        {
            var marca = _driver.FindElement(By.Id("prendaMarcaSeleccionada")); 

            //create select element object 
            SelectElement selectElement = new SelectElement(marca);
            //select Action from the dropdown menu
            selectElement.SelectByText(marcaSelected);

            _driver.FindElement(By.Id("filterbyNombrePrecioMarca")).Click();
        }

        private void Third_filter_prendas_byPrecio(int precioFilter)
        {
            _driver.FindElement(By.Id("prendaPrecio")).Clear();
            
            _driver.FindElement(By.Id("prendaPrecio")).SendKeys(precioFilter.ToString());

            _driver.FindElement(By.Id("filterbyNombrePrecioMarca")).Click();
        }


        private void Third_select_prendas_and_submit()
        {

            _driver.FindElement(By.Id("Prenda_1")).Click();
            _driver.FindElement(By.Id("Prenda_13")).Click();
            _driver.FindElement(By.Id("nextButton")).Click();

        }
        
        private void Third_alternate_not_selecting_prendas()
        {

            _driver.FindElement(By.Id("nextButton")).Click();

        }
        

        private void Fourth_fill_in_information_and_press_create(string deliveryAddress, string quantityPrenda1,
            string quantityPrenda2, string PaymentMethod, string creditCardNumber, string CCV, string expirationDate,
            string Email, string Prefix, string Phone)
        {

            _driver.FindElement(By.Id("DireccionEnvio")).SendKeys(deliveryAddress);

            _driver.FindElement(By.Id("Prenda_Cantidad_1")).Clear();
            _driver.FindElement(By.Id("Prenda_Cantidad_1")).SendKeys(quantityPrenda1);

            _driver.FindElement(By.Id("Prenda_Cantidad_13")).Clear();
            _driver.FindElement(By.Id("Prenda_Cantidad_13")).SendKeys(quantityPrenda2);

            if (PaymentMethod.Equals("CreditCard"))
            {
                _driver.FindElement(By.Id("r11")).Click();

                _driver.FindElement(By.Id("CreditCardNumber")).SendKeys(creditCardNumber);

                _driver.FindElement(By.Id("CCV")).SendKeys(CCV);

                _driver.FindElement(By.Id("ExpirationDate")).Clear();
                _driver.FindElement(By.Id("ExpirationDate")).SendKeys(expirationDate);
            }
            else
            {
                _driver.FindElement(By.Id("r12")).Click();

                _driver.FindElement(By.Id("Email")).SendKeys(Email);

                _driver.FindElement(By.Id("Prefix")).SendKeys(Prefix);

                _driver.FindElement(By.Id("Phone")).SendKeys(Phone);
            }
            _driver.FindElement(By.Id("CreateButton")).Click();
        }


        //revisar
        [Theory]
        [ClassData(typeof(PurchasePrendasTestDataGeneratorBasicFlow))]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_0_1_basic_flow(string deliveryAddress, string quantityPrenda1,
            string quantityPrenda2, string PaymentMethod, string creditCardNumber, string CCV, string expirationDate,
            string Email, string Prefix, string Phone)
        {
            //Arrange
            string[] expectedText = { "Details - ShopApp","Details",
                "Purchase","Peter","Jackson","Jackson","Fecha de Compra","Delivery Address",
                deliveryAddress,
                "Precio Total","35","Camiseta","Nike","10","2","Gorra","Adidas","15","1"};
            //Act
            Precondition_perform_login();
            First_step_accessing_purchases();
            Second_step_accessing_link_Create_New();
            Third_select_prendas_and_submit();
            Fourth_fill_in_information_and_press_create(deliveryAddress,
                quantityPrenda1, quantityPrenda2, PaymentMethod, creditCardNumber, CCV, expirationDate, Email, Prefix, Phone);

            //Assert
            foreach (string expected in expectedText)
                Assert.Contains(expected, _driver.PageSource);

        }

        [Fact(Skip = "As precondition, first execute script borrarPrendas.sql to delete prendas")]
        //[Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_2_alternate_flow_1_NoPrendasAvailable()
        {
            //Arrange
            string expectedText = "Lo sentimos, no hay prendas disponibles";

            //Act
            Precondition_perform_login();
            First_step_accessing_purchases();
            Second_step_accessing_link_Create_New();

            var prendaRow = _driver.FindElement(By.Id("NoPrendas"));

            //checks the expected row exists
            Assert.NotNull(prendaRow);
            Assert.Equal(expectedText, prendaRow.Text);
        }

        [Theory]
        [InlineData("Camiseta", "10", "Nike", "Nombre")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_3_4_alternate_flow_2_filteringbyNombre(string nombre, string precio,string marca, string filter)
        {
            //Arrange
            string[] expectedText = { nombre, precio, marca };

            //Act
            Precondition_perform_login();
            First_step_accessing_purchases();
            Second_step_accessing_link_Create_New();
            if (filter.Equals("Nombre"))
                Third_filter_prendas_byNombre(nombre);
            else
                Third_filter_prendas_byMarca(marca);

            var prendaRow = _driver.FindElements(By.Id("Prenda_Nombre_Camiseta"));

            //checks the expected row exists
            Assert.NotNull(prendaRow);

            //checks every column has the data as expected
            foreach (string expected in expectedText)
                Assert.NotNull(prendaRow.First(l => l.Text.Contains(expected)));
        }
        [Theory]
        [InlineData("Camiseta", "10", "Nike", "Marca")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_3_4_alternate_flow_2_filteringbyMarca(string nombre, string precio, string marca, string filter)
        {
            //Arrange
            string[] expectedText = { nombre, precio, marca };

            //Act
            Precondition_perform_login();
            First_step_accessing_purchases();
            Second_step_accessing_link_Create_New();
            if (filter.Equals("Marca"))
                Third_filter_prendas_byMarca(marca);
           

            var prendaRow = _driver.FindElements(By.Id("Prenda_Nombre_Camiseta"));

            //checks the expected row exists
            Assert.NotNull(prendaRow);

            //checks every column has the data as expected
            foreach (string expected in expectedText)
                Assert.NotNull(prendaRow.First(l => l.Text.Contains(expected)));
        }

        [Theory]
        [InlineData("Sudadera", "20", "Joma", "Nombre")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_3_4_alternate_flow_2_filteringbyPrecio(string nombre, string precio, string marca, string filter)
        {
            //Arrange
            string[] expectedText = { nombre, "20", marca };

            //Act
            Precondition_perform_login();
            First_step_accessing_purchases();
            Second_step_accessing_link_Create_New();
            if (filter.Equals("Precio"))
                Third_filter_prendas_byMarca(precio);


            var prendaRow = _driver.FindElements(By.Id("Prenda_Nombre_Sudadera"));

            //checks the expected row exists
            Assert.NotNull(prendaRow);

            //checks every column has the data as expected
            foreach (string expected in expectedText)
                Assert.NotNull(prendaRow.First(l => l.Text.Contains(expected)));
        }

        /*
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_5_alternate_flow_3_moviesNotSelected()
        {
            //Arrange
            string expectedText = "You must select at least one movie";

            //Act
            Precondition_perform_login();
            First_step_accessing_purchases();
            Second_step_accessing_link_Create_New();
            Third_alternate_not_selecting_movies();
            //Assert
            var errorMessage = _driver.FindElement(By.Id("ModelErrors")).Text;

            Assert.Equal(expectedText, errorMessage);

            Assert.Contains(expectedText, _driver.PageSource);

        }
        */

        [Theory]
        [InlineData("", "2", "2", "CreditCard", "1234567890123456", "123", "12/12/2022", null, null, null, "Por favor, introduzca una direccion de envio valida")]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "CreditCard", "", "123", "12/12/2022", null, null, null, "Please, fill in your Credit Card Number for your Credit Card payment")]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "CreditCard", "1234567890123456", "", "12/12/2022", null, null, null, "Please, fill in your CCV for your Credit Card payment")]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "CreditCard", "1234567890123456", "123", "", null, null, null, "Please, fill in your ExpirationDate for your Credit Card payment")]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "PayPal", null, null, null, "", "967", "673240", "Please, fill in your Email for your PayPal payment")]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "PayPal", null, null, null, "peter@uclm.com", "", "673240", "Please, fill in your Prefix for your PayPal payment")]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "2", "2", "PayPal", null, null, null, "peter@uclm.com", "967", "", "Please, fill in your Phone for your PayPal payment")]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "", "2", "CreditCard", "1234567890123456", "123", "12/12/2022", null, null, null, "The Cantidad field is required")]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "400", "2", "CreditCard", "1234567890123456", "123", "12/12/2022", null, null, null, "No hay prendas suficientes llamadas: Camiseta")]
        [InlineData("Calle de la Universidad 1, Albacete, 02006, España", "0", "0", "CreditCard", "1234567890123456", "123", "12/12/2022", null, null, null, "Por favor, selecciona mas de una prenda para realizar el pedido")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_6_UC1_6_15_alternate_flow_4_testingErrorsMandatorydata(string deliveryAddress, string quantityPrenda1, string quantityPrenda2,
            string PaymentMethod, string creditCardNumber, string CCV, string expirationDate,
            string Email, string Prefix, string Phone, string expectedText)
        {

            //Act
            Precondition_perform_login();
            First_step_accessing_purchases();
            Second_step_accessing_link_Create_New();
            Third_select_prendas_and_submit();
            Fourth_fill_in_information_and_press_create(deliveryAddress, quantityPrenda1, quantityPrenda2, PaymentMethod, creditCardNumber, CCV, expirationDate, Email, Prefix, Phone);


            //Assert
            //the expected error is shown in the view
            Assert.Contains(expectedText, _driver.PageSource);


        }
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_16_not_logged_in()
        {
            //Arrange
            string expectedText = "Use a local account to log in.";

            //Act
            First_step_accessing_purchases();
            //Assert
            Assert.Contains(expectedText, _driver.PageSource);

        }
    }




}
