﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Gummi.Models;
using Gummi.Controllers;
using System.Linq;
using Moq;

namespace Gummi.Tests.ControllerTests
{

	[TestClass]
	public class ProductsControllerTests
	{
		Mock<IProductRepository> mock = new Mock<IProductRepository>();

		private void DbSetup()
		{
			mock.Setup(m => m.Products).Returns(new Product[]
			{
				new Product {ProductId = 1, Description = "Wash the dog" },
				new Product {ProductId = 2, Description = "Do the dishes" },
				new Product {ProductId = 3, Description = "Sweep the floor" }
			}.AsQueryable());
		}


		[TestMethod]
		public void Mock_GetViewResultIndex_ActionResult() // Confirms route returns view
		{
			//Arrange
			DbSetup();
			ProductsController controller = new ProductsController(mock.Object);

			//Act
			var result = controller.Index();

			//Assert
			Assert.IsInstanceOfType(result, typeof(ActionResult));
		}

		[TestMethod]
		public void Mock_IndexContainsModelData_List() // Confirms model as list of products
		{
			// Arrange
			DbSetup();
			ViewResult indexView = new ProductsController(mock.Object).Index() as ViewResult;

			// Act
			var result = indexView.ViewData.Model;

			// Assert
			Assert.IsInstanceOfType(result, typeof(List<Product>));
		}

		[TestMethod]
		public void Mock_IndexModelContainsProducts_Collection() // Confirms presence of known entry
		{
			// Arrange
			DbSetup();
			ProductsController controller = new ProductsController(mock.Object);
			Product testProduct = new Product();
			testProduct.Description = "Wash the dog";
			testProduct.ProductId = 1;

			// Act
			ViewResult indexView = controller.Index() as ViewResult;
			List<Product> collection = indexView.ViewData.Model as List<Product>;

			// Assert
			CollectionAssert.Contains(collection, testProduct);
		}

		[TestMethod]
		public void Mock_PostViewResultCreate_ViewResult()
		{
			// Arrange
			Product testProduct = new Product
			{
				ProductId = 1,
				Description = "Wash the dog"
			};

			DbSetup();
			ProductsController controller = new ProductsController(mock.Object);

			// Act
			var resultView = controller.Create(testProduct) as ViewResult;


			// Assert
			Assert.IsInstanceOfType(resultView, typeof(ViewResult));

		}
		[TestMethod]
		public void Mock_GetDetails_ReturnsView()
		{
			// Arrange
			Product testProduct = new Product
			{
				ProductId = 1,
				Description = "Wash the dog"
			};

			DbSetup();
			ProductsController controller = new ProductsController(mock.Object);

			// Act
			var resultView = controller.Details(testProduct.ProductId) as ViewResult;
			var model = resultView.ViewData.Model as Product;

			// Assert
			Assert.IsInstanceOfType(resultView, typeof(ViewResult));
			Assert.IsInstanceOfType(model, typeof(Product));
		}
	}
}