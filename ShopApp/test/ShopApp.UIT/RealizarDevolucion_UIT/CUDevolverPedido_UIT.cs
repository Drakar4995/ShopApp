using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using ShopApp.UIT.RealizarDevolucion_UIT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace ShopApp.UIT.DevolverPedido_UIT
{
    public class CUDevolverPedido_UIT : IDisposable
    {
        //int i = 0; 
        IWebDriver _driver;
        string _URI;

        public void Dispose()
        {
            _driver.Close();
            _driver.Dispose();
            GC.SuppressFinalize(this);
        }
        public CUDevolverPedido_UIT()
        {
            UtilitiesUIT.SetUp_UIT(out _driver, out _URI);


        }

        [Fact]
        public void Initial_step_opening_the_web_page()
        {
            //Arange
            string expectedTitle = "Home Page - ShopApp";
            string expectedText = "Register";
            //Act
            _driver.Navigate().GoToUrl(_URI);

            //Assert
            Assert.Equal(expectedTitle, _driver.Title);
            Assert.Contains(expectedText, _driver.PageSource); //PageSource contiene todos los elementos de la pagina
        }


        public void precondicion_logearse()
        {
            _driver.Navigate().GoToUrl(_URI + "Identity/Account/Login");
            _driver.FindElement(By.Id("Input_Email")).SendKeys("peter@uclm.com");
            _driver.FindElement(By.Id("Input_Password")).SendKeys("OtherPass12$");
            _driver.FindElement(By.Id("login-submit")).Click();
        }

        public void acceder_a_Devoluciones()
        {
            _driver.FindElement(By.Id("botonDevoluciones")).Click();

        }
        public void crear_Devolucion()
        {
            _driver.FindElement(By.Id("botonRealizaDevolucion")).Click();

        }
        public void seleccionar_Devolucion(int id_Compra)
        {
            string id = "CompraId_" + id_Compra.ToString();
            _driver.FindElement(By.Id(id)).Click();
        }
        public void filtrar_por_nombre_Prenda(string nombrePrenda)
        {

            _driver.FindElement(By.Id("nombrePrenda")).SendKeys(nombrePrenda);
            _driver.FindElement(By.Id("botonFiltrar")).Click();
        }
        public void filtrar_por_nombre_Marca(string nombreMarca)
        {

            var marcas = _driver.FindElement(By.Id("marcaSelect"));
            //Guardamos las marcas a seleccionar 
            SelectElement selectElement = new SelectElement(marcas);
            //Seleccionamos la marca por el nombreMarca
            selectElement.SelectByText(nombreMarca);
            //Click en el boton de filtrar
            _driver.FindElement(By.Id("botonFiltrar")).Click();
        }
        public void no_Seleccion_Prendas_a_Devolver()
        {
            _driver.FindElement(By.Id("nextButton")).Click();
        }
        public void seleccionar_Prenda_a_Devolver(int idPrenda)
        {
            _driver.FindElement(By.Id("ItemCompras_" + idPrenda.ToString())).Click();
        }

        [Fact]
        public void Caso_Prueba_UC3_filtrar_por_nombrePrenda()
        {
            //Arrange
            string prendaNombre = "Camiseta";
            string[] expectedText = { "Camiseta", "1", "Nike", "5" };
            int id_Compra = 6;
            int idPedido = 9;

            //Act
            Initial_step_opening_the_web_page();
            precondicion_logearse();
            acceder_a_Devoluciones();
            crear_Devolucion();
            seleccionar_Devolucion(id_Compra); //Introducir Id_Compra a devolver
            filtrar_por_nombre_Prenda(prendaNombre);
            //Assert

            var filaPrenda = _driver.FindElements(By.Id("Id_Pedido_" + idPedido.ToString()));

            //Check if filaPrenda notnull
            Assert.NotNull(filaPrenda);

            foreach (string expected in expectedText)
                Assert.NotNull(filaPrenda.First(g => g.Text.Contains(expected)));
        }

        [Fact]
        public void Caso_Prueba_UC4_filtrar_por_nombreMarca()
        {
            //Arrange
            string marcaNombre = "Versache";
            string[] expectedText = { "Sudadera", "1", "Versache", "200" };
            int id_Compra = 6;
            int idPedido = 10;

            //Act
            Initial_step_opening_the_web_page();
            precondicion_logearse();
            acceder_a_Devoluciones();
            crear_Devolucion();
            seleccionar_Devolucion(id_Compra); //Introducir Id_Compra a devolver
            filtrar_por_nombre_Marca(marcaNombre);
            //Assert

            //Guardo una Fila
            var filaPrenda = _driver.FindElements(By.Id("Id_Pedido_" + idPedido.ToString()));

            //Check if filaPrenda notnull
            Assert.NotNull(filaPrenda);

            //Buscamos encada Fila que coincida lo que buscamos
            foreach (string expected in expectedText)
                Assert.NotNull(filaPrenda.First(g => g.Text.Contains(expected)));
        }
        [Fact(Skip ="Ejecutar Script RemoveCompras.sql")]
        //[Fact]
        public void Flujo_Alternativo_4_Esc_2_No_Compras()
        {
            //Arrange
            string expectedText = "No se han realizado Compras";
            //Act
            Initial_step_opening_the_web_page();
            precondicion_logearse();
            _driver.FindElement(By.Id("ComprasController")).Click();
            var compraRow = _driver.FindElement(By.Id("NoCompras"));
            //Assert
            Assert.NotNull(compraRow);
            Assert.Equal(expectedText, compraRow.Text);
            //NoCompras

        }

       
        [Fact]
        public void Caso_Prueba_UC5_ninguna_prenda_seleccionada()
        {
            //Arrange
            int id_Compra = 6;
            string expectedText = "Tienes que elegir al menos una prenda";
            //Act
            Initial_step_opening_the_web_page();
            precondicion_logearse();
            acceder_a_Devoluciones();
            crear_Devolucion(); 
            seleccionar_Devolucion(id_Compra);
            no_Seleccion_Prendas_a_Devolver();
            //Assert
            var errorMessage = _driver.FindElement(By.Id("ModelErrors")).Text;

            Assert.Equal(expectedText, errorMessage); //Comparamos los errores

            Assert.Contains(expectedText, _driver.PageSource); //Vemos si el texto esta en la web

        }


        [Theory]
        [ClassData(typeof(DevolucionPrendasTestDataGeneratorBasicFlow))]
        [Trait("LevelTesting", "Funcional Testing")]

        public void UC1_0_1_Basic_Flow(
            string tipoRecogida,
            string domicilio,
            string motivoPrenda1,
            string motivoPrenda2,
            string PaymentMethod,
            string creditCardNumber,
            string CCV,
            string expirationDate,
            string Email,
            string Prefix, string Phone)
        {
            //Arange
            int id_Compra = 7;
            int id_Prenda1 = 11;
            int id_Prenda2 = 12;

            string[] expectedText = {
                "Details",
                "Devolucion",
                "Total a Devolver","25",
                "Tarjeta / PayPal","CreditCard",
                "Numero Tarjeta","1234567890123456",
                "CCV","123",
                "Fecha de Caducidad","dic./2023",
                "Recogido en","Correos",
                "Prenda1","motivoP1","5","1",
                "Prenda4","motivoP2","20","1"
            };
            //Act
            Initial_step_opening_the_web_page();
            precondicion_logearse();
            acceder_a_Devoluciones();
            crear_Devolucion();
            seleccionar_Devolucion(id_Compra); //Introducir Id_Compra a devolver
            seleccionar_Prenda_a_Devolver(id_Prenda1);
            seleccionar_Prenda_a_Devolver(id_Prenda2);
            _driver.FindElement(By.Id("nextButton")).Click();
            //string [] idsPrendas = {"2", "3"};
            InsertarDatos_Basic_Flow2(tipoRecogida, domicilio, motivoPrenda1, motivoPrenda2, PaymentMethod, creditCardNumber, CCV, expirationDate, Email, Prefix, Phone);
            
            //Assert
            foreach (string expected in expectedText)
                Assert.Contains(expected, _driver.PageSource);

        }


        [Theory]
        [ClassData(typeof(DevolucionPrendasTestDataGeneratorBasicFlowPayPal))]
        [Trait("LevelTesting", "Funcional Testing")]

        public void UC1_0_1_Basic_Flow_PayPal(
            string tipoRecogida,
            string domicilio,
            string motivoPrenda1,
            string motivoPrenda2,
            string PaymentMethod,
            string creditCardNumber,
            string CCV,
            string expirationDate,
            string Email,
            string Prefix, string Phone)
        {
            //Arange
            int id_Compra = 8;
            int id_Prenda1 = 13;
            //int id_Prenda2 = 4;

            string[] expectedText = {
                "Details",
                "Devolucion",
                "Total a Devolver","10",
                "Tarjeta / PayPal","PayPal",
                "Correo Electronico","peter@uclm.com",
                "Prefijo","967",
                "Telefono","673240",
                "Recogido en","Calle 1",
                "Camiseta","motivoP1","10","1",
               
            };
            //Act
            Initial_step_opening_the_web_page();
            precondicion_logearse();
            acceder_a_Devoluciones();
            crear_Devolucion();
            seleccionar_Devolucion(id_Compra); //Introducir Id_Compra a devolver
            seleccionar_Prenda_a_Devolver(id_Prenda1);
            //seleccionar_Prenda_a_Devolver(id_Prenda2);
            _driver.FindElement(By.Id("nextButton")).Click();

            InsertarDatosPayPal(tipoRecogida, domicilio, motivoPrenda1, motivoPrenda2, PaymentMethod, creditCardNumber, CCV, expirationDate, Email, Prefix, Phone);

            //Assert
            foreach (string expected in expectedText)
                Assert.Contains(expected, _driver.PageSource);

        }
        public void InsertarDatos_Basic_Flow(
            string tipoRecogida,
            string domicilio,
            string motivoPrenda1,
            string motivoPrenda2,
            string PaymentMethod,
            string creditCardNumber,
            string CCV,
            string expirationDate,
            string Email,
            string Prefix, string Phone)
        {
            if(tipoRecogida == "Correos")
            {
                _driver.FindElement(By.Id("r13")).Click();
            }
            if(tipoRecogida =="Domicilio")
            {
                _driver.FindElement(By.Id("r14")).Click();
                _driver.FindElement(By.Id("Direccion")).SendKeys(domicilio);
            }
            
            _driver.FindElement(By.Id("itemDevolucion_9")).SendKeys(motivoPrenda1);
            //_driver.FindElement(By.Id("itemDevolucion_0")).SendKeys(motivoPrenda2);



            _driver.FindElement(By.Id("itemDevolucion_10")).SendKeys(motivoPrenda2);
            
            if (PaymentMethod == "CreditCard")
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

        public void InsertarDatos_Basic_Flow2(
            string tipoRecogida,
            string domicilio,
            string motivoPrenda1,
            string motivoPrenda2,
            string PaymentMethod,
            string creditCardNumber,
            string CCV,
            string expirationDate,
            string Email,
            string Prefix, string Phone)
        {
            if (tipoRecogida == "Correos")
            {
                _driver.FindElement(By.Id("r13")).Click();
            }
            if (tipoRecogida == "Domicilio")
            {
                _driver.FindElement(By.Id("r14")).Click();
                _driver.FindElement(By.Id("Direccion")).SendKeys(domicilio);
            }

            _driver.FindElement(By.Id("itemDevolucion_11")).SendKeys(motivoPrenda1);
            //_driver.FindElement(By.Id("itemDevolucion_0")).SendKeys(motivoPrenda2);



            _driver.FindElement(By.Id("itemDevolucion_12")).SendKeys(motivoPrenda2);

            if (PaymentMethod == "CreditCard")
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
        public void InsertarDatosPayPal(
            string tipoRecogida,
            string domicilio,
            string motivoPrenda1,
            string motivoPrenda2,
            string PaymentMethod,
            string creditCardNumber,
            string CCV,
            string expirationDate,
            string Email,
            string Prefix, string Phone)
        {
            if (tipoRecogida == "Correos")
            {
                _driver.FindElement(By.Id("r13")).Click();
            }
            if (tipoRecogida == "Domicilio")
            {
                _driver.FindElement(By.Id("r14")).Click();
                _driver.FindElement(By.Id("Direccion")).SendKeys(domicilio);
            }

            _driver.FindElement(By.Id("itemDevolucion_13")).SendKeys(motivoPrenda1);
            
            //_driver.FindElement(By.Id("itemDevolucion_1")).SendKeys(motivoPrenda2);
            
            if (PaymentMethod == "CreditCard")
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
       
        [Theory]
        //Falta introducir un Motivo Devolucion
        [InlineData("Correos",null,"", "motivoP2", "CreditCard", "1234567890123456", "123", "12/12/2022", null, null, null, "Introduzca un Motivo")]
        //No ha seleccionado un tipo Recogida
        [InlineData(null, null, "motivoP1", "motivoP2", "CreditCard", "1234567890123456", "123", "12/12/2022", null, null, null, "Seleccione un tipo de Recogida")]
        //Ha seleccionado Recogida en Domicilio pero no ha introducido la direccion
        [InlineData("Domicilio", "", "motivoP1", "motivoP2", "CreditCard", "1234567890123456", "123", "12/12/2022", null, null, null, "Introduza una direccion Valida")]
        //No ha introducido un numero de tarjeta
        [InlineData("Correos", null, "motivoP1", "motivoP2", "CreditCard","", "123", "12/12/2022", null, null, null, "Please, fill in your Credit Card Number for your Credit Card payment")]
        //No ha introducido el CCV
        [InlineData("Correos", null, "motivoP1", "motivoP2", "CreditCard", "1234567890123456", "", "12/12/2022", null, null, null, "Please, fill in your CCV for your Credit Card payment")]
        //No ha introducido la Fecha
        [InlineData("Correos", null, "motivoP1", "motivoP2", "CreditCard", "1234567890123456", "123","", null, null, null, "Please, fill in your ExpirationDate for your Credit Card payment")]
        //La Fecha de La Tarjeta esta caducada 
        [InlineData("Correos", null, "motivoP1", "motivoP2", "CreditCard", "1234567890123456", "123", "12/03/2010", null, null, null, "La Tarjeta esta Caducada")]
        //PayPal Email Vacio
        [InlineData("Correos", null, "motivoP1", "motivoP2", "PayPal",null, null, null, "", "967", "673240", "Please, fill in your Email for your PayPal payment")]
        //PayPal Prefix Vacio
        [InlineData("Correos", null, "motivoP1", "motivoP2", "PayPal", null, null, null, "peter@uclm.com", "", "673240", "Please, fill in your Prefix for your PayPal payment")]
        //PayPal Phone Vacio
        [InlineData("Correos", null, "motivoP1", "motivoP2", "PayPal", null, null, null, "peter@uclm.com", "967", "", "Please, fill in your Phone for your PayPal payment")]
       
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_6_15_Flujo_Alternativo(
            string tipoRecogida,
            string domicilio,
            string motivoPrenda1,
            string motivoPrenda2,
            string PaymentMethod,
            string creditCardNumber,
            string CCV,
            string expirationDate,
            string Email,
            string Prefix, string Phone, string expectedError)
        {
            //Arrange
            int id_Compra = 6;
            int id_Prenda1 = 9;
            int id_Prenda2 = 10;  
            Initial_step_opening_the_web_page();
            precondicion_logearse();
            acceder_a_Devoluciones();
            crear_Devolucion();
            seleccionar_Devolucion(id_Compra); //Introducir Id_Compra a devolver
            seleccionar_Prenda_a_Devolver(id_Prenda1);
            seleccionar_Prenda_a_Devolver(id_Prenda2);
            _driver.FindElement(By.Id("nextButton")).Click();

            InsertarDatos_Basic_Flow(tipoRecogida, domicilio, motivoPrenda1, motivoPrenda2, PaymentMethod, creditCardNumber, CCV, expirationDate, Email, Prefix, Phone);
            //Act
            //Assert
            Assert.Contains(expectedError, _driver.PageSource);
        }
       

    }
}
