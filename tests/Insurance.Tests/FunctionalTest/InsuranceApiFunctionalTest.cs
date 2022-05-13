using Insurance.Api.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Insurance.Api.Controllers.HomeController;

namespace Insurance.Tests.FunctionalTest
{
    public class InsuranceApiFunctionalTest
    {
        [Fact]
        public void CalculateInsurance_GivenSalesPriceBetween500And2000Euros_ShouldAddThousandEurosToInsuranceCost()
        {

            var client = new HttpClient
            {
                BaseAddress = new Uri(TestConstants.INSURANCE_API_URL)
            };

            var payload = new InsuranceRequestDto()
            {
                ProductId = 858421
            };

            var response = client.PostAsync(string.Format("/api/insurance/product"),
                JsonContent.Create(payload)
                ).Result;

            response.EnsureSuccessStatusCode();

            var json = response.Content.ReadAsStringAsync().Result;

            var result = JsonConvert.DeserializeObject<InsuranceResponseDto>(json);

            Assert.Equal(1500, result.InsuranceValue);

        }

        [Fact]
        public void CalculateInsurance_GivenLaptopCostsLess500_Should500InsuranceCost()
        {

            var client = new HttpClient
            {
                BaseAddress = new Uri(TestConstants.INSURANCE_API_URL)
            };

            var prodId = ProductApiHelper.GetProductPerTypeAndSalesPrice(TestConstants.LAPTOP_TYPE_ID, 0, 499);
            var payload = new InsuranceRequestDto()
            {
                ProductId = prodId
            };

            var response = client.PostAsync("/api/insurance/product",
                JsonContent.Create(payload)
                ).Result;

            response.EnsureSuccessStatusCode();

            var json = response.Content.ReadAsStringAsync().Result;

            var result = JsonConvert.DeserializeObject<InsuranceResponseDto>(json);

            Assert.Equal(500, result.InsuranceValue);

        }

        [Fact]
        public void CalculateOrderInsurance_GivenTVItemsMoreThan500LessThan2000_ShouldInsurance1000PerItem()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(TestConstants.INSURANCE_API_URL)
            };

            int[] prodIds = ProductApiHelper.GetProductsPerTypeAndSalesPrice(2, productTypeId: 124,
                minSalesPrice: 500, maxSalesPrice: 1999);
            var payload = new OrderDto();
            foreach (int id in prodIds)
            {
                payload.Items.Add(new OrderItemDto(id, 1));
            };

            var response = client.PostAsync("/api/insurance/order",
                JsonContent.Create(payload)
                ).Result;

            response.EnsureSuccessStatusCode();

            var json = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<OrderResultDto>(json);

            int expectedInsurance = 1000 * payload.Items.Count;
            Assert.Equal(
                expected: expectedInsurance,
                actual: result.InsuranceValue);
        }

        [Fact]
        public void CalculateOrderInsurance_GivenCameraItems_ShouldInsuranceByMoreThan500()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(TestConstants.INSURANCE_API_URL)
            };

            int id = ProductApiHelper.GetProductPerType(TestConstants.DIGITAL_CAMERA_TYPE_ID);
            var payload = new OrderDto { Items = new List<OrderItemDto> { new OrderItemDto(id, 10) } };

            var response = client.PostAsync("/api/insurance/order",
                JsonContent.Create(payload)
                ).Result;

            response.EnsureSuccessStatusCode();

            var json = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<OrderResultDto>(json);

            Assert.True(result.InsuranceValue >= 500);
        }

        [Fact]
        public void SaveSurchargeRate_GivenLaptopSurcharge_ShouldReturnOK()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(TestConstants.INSURANCE_API_URL)
            };

            var payload = new SurchargeRequestDto {
                ProductTypeId = TestConstants.LAPTOP_TYPE_ID,
                SurchargeRate = 300
            };

            var response = client.PostAsync("/api/admin/surcharge/producttype",
                JsonContent.Create(payload)
                ).Result;

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void SaveSurchargeRate_GivenOrderWith700SurchargeItemUnder500_ShouldInsurance1200()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(TestConstants.INSURANCE_API_URL)
            };

            // Save Surcharge
            var payload = new SurchargeRequestDto
            {
                ProductTypeId = TestConstants.LAPTOP_TYPE_ID,
                SurchargeRate = 700
            };
            var response = client.PostAsync("/api/admin/surcharge/producttype",
                JsonContent.Create(payload)
                ).Result;
            response.EnsureSuccessStatusCode();

            // Calculate order insurance
            int id = ProductApiHelper.GetProductPerTypeAndSalesPrice(TestConstants.LAPTOP_TYPE_ID, 1, 499);
            var orderPayload = new OrderDto { Items = new List<OrderItemDto> { new OrderItemDto(id, 1) } };

            var orderResponse = client.PostAsync("/api/insurance/order",
                JsonContent.Create(orderPayload)
                ).Result;

            orderResponse.EnsureSuccessStatusCode();

            var json = orderResponse.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<OrderResultDto>(json);

            Assert.Equal(1200, result.InsuranceValue);
        }
    }
}
