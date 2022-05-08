using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace ShopApp.UIT.Suscripciones
{
    public class UC_SuscribirseNewsLetter_UIT : IDisposable
    {

        IWebDriver _driver;
        string _URI;

        public UC_SuscribirseNewsLetter_UIT()
        {
            UtilitiesUIT.SetUp_UIT(out _driver, out _URI);
            initial_step_opening_the_web_page();
        }

        public void Dispose()
        {
            _driver.Close();
            _driver.Dispose();
            GC.SuppressFinalize(this);
            
        }

        
        public void initial_step_opening_the_web_page()
        {
            //Arrange
            string expectedTitle = "Home Page - ShopApp";
            string expectedText = "Register";

            //ACT
            _driver.Navigate().GoToUrl(_URI);

            //Assert
            Assert.Equal(expectedTitle, _driver.Title);
            Assert.Contains(expectedText, _driver.PageSource);
            //throw new NotImplementedException();
        }

        
        public void precondicion_loggearse()
        {
            _driver.Navigate().GoToUrl(_URI + "Identity/Account/Login");
            _driver.FindElement(By.Id("Input_Email")).SendKeys("Peter@uclm.com");
            _driver.FindElement(By.Id("Input_Password")).SendKeys("OtherPass12$");
            _driver.FindElement(By.Id("login-submit")).Click();
        }

        
        private void First_step_accediendo_suscripciones()
        {
            _driver.FindElement(By.Id("SuscripcionsController")).Click();

        }

        
        private void Second_step_accediendo_link_Create_New()
        {
        
            _driver.FindElement(By.LinkText("Nueva suscripcion")).Click();

        }

        
        private void Third_filter_newsletter_byCategoria(string CategoriaFiltro)
        {
            _driver.FindElement(By.Id("newsletterCategoria")).SendKeys(CategoriaFiltro);

            _driver.FindElement(By.Id("filterbyTitleCategoria")).Click();
        }

        private void Third_filter_newsletter_byMarca(string MarcaSelected)
        {

            var marca = _driver.FindElement(By.Id("newslettermarcaselected"));

            //create select element object 
            SelectElement selectElement = new SelectElement(marca);
            //select Action from the dropdown menu
            selectElement.SelectByText(MarcaSelected);

            _driver.FindElement(By.Id("filterbyTitleCategoria")).Click();

        }

        private void Third_select_newsletter_and_submit()
        {

            _driver.FindElement(By.Id("Newsletter_1")).Click();
            _driver.FindElement(By.Id("Newsletter_2")).Click();
            _driver.FindElement(By.Id("nextButton")).Click();

        }

        private void Third_alternate_not_selecting_newsletter()
        {

            _driver.FindElement(By.Id("nextButton")).Click();

        }

        private void Fourth_fill_in_information_and_press_create(string titulo, string descripcion,
           string motivo, string fechaCaducidad)
        {

            _driver.FindElement(By.Id("Titulo")).SendKeys(titulo);

            _driver.FindElement(By.Id("Descripcion")).SendKeys(descripcion);

            _driver.FindElement(By.Id("Motivo")).SendKeys(motivo);

            _driver.FindElement(By.Id("FechaCaducidad")).Clear();
            _driver.FindElement(By.Id("FechaCaducidad")).SendKeys(fechaCaducidad);


            _driver.FindElement(By.Id("CreateButton")).Click();
        }

        [Theory]
        [ClassData(typeof(SuscripcionNewsletterTestDataGeneratorBasicFlow))]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC4_0_1_basic_flow(string titulo, string descripcion,
           string motivo, string fechaCaducidad)
        {
            //Arrange
            string[] expectedText = { "Details - ShopApp","Details", "Titulo","Descripcion","FechaCaducidad","Nike","Adidas",
                                                                                    titulo,descripcion,fechaCaducidad};
            //Act
            precondicion_loggearse();
            First_step_accediendo_suscripciones();
            Second_step_accediendo_link_Create_New();
            Third_select_newsletter_and_submit();
            Fourth_fill_in_information_and_press_create(titulo, descripcion,
           motivo, fechaCaducidad);

            //Assert
            foreach (string expected in expectedText)
               Assert.Contains(expected, _driver.PageSource);

        }

        //Como precondicion ejecutar el scrip dbo.NoNewsLetter.data.sql
        [Fact(Skip = "Antes hay que ejecutar el scrip dbo.NoNewsLetter.data.sql")]
        //[Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC4_2_alternate_flow_1_NoNewsLetterDispoonibles()
        {
            //Arrange
            
            string expectedText = "There are no Newsletter available";

            //Act
            precondicion_loggearse();
            First_step_accediendo_suscripciones();
            Second_step_accediendo_link_Create_New();

            var NewsLetterRow = _driver.FindElement(By.Id("NoNewsletter"));

            //checks the expected row exists
            Assert.NotNull(NewsLetterRow);
            Assert.Equal(expectedText, NewsLetterRow.Text);
        }

        [Theory]
        [InlineData("Newsletter1", "Nike", "Originals", "Categoria")]
        [InlineData("Newsletter2", "Adidas", "Kids", "Marca")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC4_3_4_alternate_flow_2_filteringbyCategoria(string titulo, string marca,
            string categoria, string filter)
        {
            //Arrange
            string[] expectedText = { titulo, marca, categoria };

            //Act
            precondicion_loggearse();
            First_step_accediendo_suscripciones();
            Second_step_accediendo_link_Create_New();
            if (filter.Equals("Categoria"))
                Third_filter_newsletter_byCategoria(categoria);
            else
                Third_filter_newsletter_byMarca(marca);

            var newsLetterRow = _driver.FindElements(By.Id("Newsletter_Title_" + titulo));

            //checks the expected row exists
            Assert.NotNull(newsLetterRow);

            //checks every column has the data as expected
            foreach (string expected in expectedText)
                Assert.NotNull(newsLetterRow.First(l => l.Text.Contains(expected)));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC4_5_alternate_flow_3_newsLetterNotSelected()
        {
            //Arrange
            string expectedText = "You must select at least one newsletter";

            //Act
            precondicion_loggearse();
            First_step_accediendo_suscripciones();
            Second_step_accediendo_link_Create_New();
            Third_alternate_not_selecting_newsletter();
            //Assert
            var errorMessage = _driver.FindElement(By.Id("ModelErrors")).Text;

            Assert.Equal(expectedText, errorMessage);

            Assert.Contains(expectedText, _driver.PageSource);

        }

        [Theory]
        [InlineData("", "Descripcion1", "Motivo1", "05/06/2022", "The Titulo field is required.")]
        [InlineData("Suscripcion1", "", "Motivo1", "05/06/2022", "The Descripcion de la suscripcion field is required.")]
        [InlineData("Suscripcion1", "Descripcion1", "", "05/06/2022", "The Motivo de la suscripcion field is required.")]
        [InlineData("Suscripcion1", "Descripcion1", "Motivo1", "", "The value '' is invalid.")]
        [InlineData("Suscripcion1", "Descripcion1", "Motivo1", "05/06/2000", "Fecha no valida")]
        public void UC6_7_8_alternate_flow_4_testingErrorsMandatorydata(string titulo, string descripcion,
           string motivo, string fechaCaducidad,string expectedText)
        {
            
            //Act
            precondicion_loggearse();
            First_step_accediendo_suscripciones();
            Second_step_accediendo_link_Create_New();
            Third_select_newsletter_and_submit();
            Fourth_fill_in_information_and_press_create(titulo, descripcion,
           motivo, fechaCaducidad);

            //Assert
            Assert.Contains(expectedText, _driver.PageSource);

        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC4_11_not_logged_in()
        {
            //Arrange
            string expectedText = "Use a local account to log in.";

            //Act
            First_step_accediendo_suscripciones();
            
            //Assert
            Assert.Contains(expectedText, _driver.PageSource);

        }

    }
}
