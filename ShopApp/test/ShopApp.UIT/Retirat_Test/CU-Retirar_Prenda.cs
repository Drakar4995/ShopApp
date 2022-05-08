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



namespace ShopApp.UIT.Retirat_Test
{
    public class CU_Retirar_Prenda : IDisposable
    {
        IWebDriver _driver;
        string _URI;
        public CU_Retirar_Prenda()
        {
            UtilitiesUIT.SetUp_UIT(out _driver, out _URI);
            initial_step_opening_the_web_page();
        }

        public void initial_step_opening_the_web_page()
        {
            //Arrange
            string expectedTitle = "Home Page - ShopApp";
            string expectedText = "Register";
            //Act
            //El navegador cargará la URI indicada
            _driver.Navigate().GoToUrl(_URI);
            //Assert
            //Comprueba que el título coincide con el esperado
            Assert.Equal(expectedTitle, _driver.Title);
            //Comprueba si la página contiene el string indicado
            Assert.Contains(expectedText, _driver.PageSource);
        }

        public void prediccion_loggearse()
        {
            _driver.Navigate().GoToUrl(_URI + "Identity/Account/Login");
            _driver.FindElement(By.Id("Input_Email")).SendKeys("joseAngel@uclm.com");
            _driver.FindElement(By.Id("Input_Password")).SendKeys("APassword1234%");
            _driver.FindElement(By.Id("login-submit")).Click();

        }

        public void primer_paso()
        {
            _driver.FindElement(By.Id("RetiradasController")).Click();
        }

        public void segundo_paso()
        {
            _driver.FindElement(By.Id("SelectPrendasForRetirar")).Click();

        }

        public void tercer_paso_filtrar_por_ventasSemanales(int ventasSemanales)
        {
            _driver.FindElement(By.Id("prendaVentasSemanales")).Clear();
            _driver.FindElement(By.Id("prendaVentasSemanales")).SendKeys(ventasSemanales.ToString());
            _driver.FindElement(By.Id("filterbyVentasSemanalesMarca")).Click();

        }

        public void tercer_paso_filtrar_por_Marca(string marcaselected)
        {
            var marca = _driver.FindElement(By.Id("prendaMarcaSelected"));
            SelectElement selectElement = new SelectElement(marca);
            selectElement.SelectByText(marcaselected);
            _driver.FindElement(By.Id("filterbyVentasSemanalesMarca")).Click();
        }
        public void seleccionar_prenda()
        {
            _driver.FindElement(By.Id("Prenda_14")).Click();
            _driver.FindElement(By.Id("nextButton")).Click();

        }
        public void seleccionar_prenda_1()
        {
            _driver.FindElement(By.Id("Prenda_2")).Click();
            _driver.FindElement(By.Id("nextButton")).Click();

        }
        public void Third_alternate_not_selecting_prenda()
        {

            _driver.FindElement(By.Id("nextButton")).Click();

        }

        public void rellenar_pulsar_create(string descripcion, string fecharetirada, string motivoretirada)
        {
            _driver.FindElement(By.Id("Descripcion")).SendKeys(descripcion);

            _driver.FindElement(By.Id("FechaEfectiva")).Clear();
            _driver.FindElement(By.Id("FechaEfectiva")).SendKeys(fecharetirada);

            _driver.FindElement(By.Id("Movie_Quantity_14")).Clear();
            _driver.FindElement(By.Id("Movie_Quantity_14")).SendKeys(motivoretirada);

            _driver.FindElement(By.Id("CreateButton")).Click();
        
    }
        public void rellenar_pulsar_create_2(string descripcion, string fecharetirada, string motivoretirada)
        {
            _driver.FindElement(By.Id("Descripcion")).SendKeys(descripcion);

            _driver.FindElement(By.Id("FechaEfectiva")).Clear();
            _driver.FindElement(By.Id("FechaEfectiva")).SendKeys(fecharetirada);

            _driver.FindElement(By.Id("Movie_Quantity_2")).Clear();
            _driver.FindElement(By.Id("Movie_Quantity_2")).SendKeys(motivoretirada);

            _driver.FindElement(By.Id("CreateButton")).Click();

        }


        [Fact]
        public void Caso_prueba_UC3_filtrar_por_ventas()
        {
            //Arrange
            int ventasparafiltrar = 1;
            string[] expectedText = { "Sudadera", "Joma", "20", "0" };
            
            //Act
            initial_step_opening_the_web_page();
            prediccion_loggearse();
            primer_paso();
            segundo_paso();
            tercer_paso_filtrar_por_ventasSemanales(ventasparafiltrar);

            //Assert
            var prendaRow = _driver.FindElements(By.Id("Newsletter_Title_Sudadera"));
            //checks the expected row exists
            Assert.NotNull(prendaRow);
            //checks every column has those data expected
            foreach (string expected in expectedText)
                Assert.NotNull(prendaRow.First(l => l.Text.Contains(expected)));
        }
        [Fact]
        public void Caso_prueba_UC3_filtrar_por_marcas()
        {
            //Arrange
            string marcaselected = "Joma";
            string[] expectedText = { "Sudadera", "Joma", "20", "0" };

            //Act
            initial_step_opening_the_web_page();
            prediccion_loggearse();
            primer_paso();
            segundo_paso();
            tercer_paso_filtrar_por_Marca(marcaselected);

            //Assert
            var prendaRow = _driver.FindElements(By.Id("Newsletter_Title_Sudadera"));
            //checks the expected row exists
            Assert.NotNull(prendaRow);
            //checks every column has those data expected
            foreach (string expected in expectedText)
                Assert.NotNull(prendaRow.First(l => l.Text.Contains(expected)));
        }
        [Fact]
        public void UC3_1_no_prenda_selected()
        {
            //Arrange
            string expectedText = "Tienes que elegir al menos una prenda";
            //Act
            initial_step_opening_the_web_page();
            prediccion_loggearse();
            primer_paso();
            segundo_paso();
            Third_alternate_not_selecting_prenda();
            //Assert
            var errorMessage = _driver.FindElement(By.Id("ModelErrors")).Text;

            Assert.Equal(expectedText, errorMessage);

            Assert.Contains(expectedText, _driver.PageSource);

        }

        [Fact(Skip = "As precondition, first execute script no_prenda.data.sql to delete prendas")]
        //[Fact]
        public void UC3_1_no_prendas()
        {
            //Arrange
            string expectedText = "No hay prendas disponibles";
            //Act
            initial_step_opening_the_web_page();
            prediccion_loggearse();
            primer_paso();
            segundo_paso();

            var prendaRow = _driver.FindElement(By.Id("NoPrenda"));

            //checks the expected row exists
            Assert.NotNull(prendaRow);
            Assert.Equal(expectedText, prendaRow.Text);

        }
        [Theory]
        [ClassData(typeof(Retirar_Prendas_BasicFlow))]
        public void UC3_1_basic_flow(string descripcion, string fecharetirada, string motivoretirada)
        {
            //Arrange
            string[] expectedText = {
                "Details",
                "Retirada",
                "Titulo", "Retirada1",
                "Descripcion",descripcion,
                "Fecha Efectiva",fecharetirada,
                "Pantalon","Nike","28",motivoretirada};
            //Act
            initial_step_opening_the_web_page();
            prediccion_loggearse();
            primer_paso();
            segundo_paso();
            seleccionar_prenda();
            rellenar_pulsar_create(descripcion, fecharetirada, motivoretirada);
            //Assert
            foreach (string expected in expectedText)
                Assert.Contains(expected, _driver.PageSource);

        }

        [Theory]
        [InlineData("", "16/12/2021", "Motivo1", "Por favor, escribe una descripcion")]
        [InlineData("Monte Olimpo", "", "Motivo1" ,"Introduce una fecha")]
        [InlineData("Monte Olimpo", "16/12/2021", "", "Escribe un motivo")]
        [InlineData("Monte Olimpo", "16/12/2000", "Motivo1", "Fecha no valida")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC3_5_8_Flujo_Alternativo(
            string descripcion, 
            string fecharetirada, 
            string motivoretirada,
            string expectedError)
        {
            //Arrange
            initial_step_opening_the_web_page();
            prediccion_loggearse();
            primer_paso();
            segundo_paso();
            seleccionar_prenda_1();
            rellenar_pulsar_create_2(descripcion, fecharetirada, motivoretirada);
            //Act
            //Assert
            Assert.Contains(expectedError, _driver.PageSource);

        }
        [Fact]
        public void UC3_sesion_no_iniciada()
        {
            //Arrange
            string expectedText = "Use a local account to log in";

            //Act
            primer_paso();
            //Assert
            Assert.Contains(expectedText, _driver.PageSource);

        }

        public void Dispose()
        {
            _driver.Close();
            _driver.Dispose();
            GC.SuppressFinalize(this);
            
        }
    }
}
