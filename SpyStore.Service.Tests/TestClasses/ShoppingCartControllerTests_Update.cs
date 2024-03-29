﻿using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using SpyStore.Service.Tests.Helpers;
using Xunit;

namespace SpyStore.Service.Tests.TestClasses
{
    public partial class ShoppingCartRecordControllerTests 
    {
        [Fact]
        public async void ShouldUpdateRecordInTheCart()
        {
            //Change Cart Item(Quantity): http://localhost:55882/api/shoppingcart/{customerId}/{shoppingCartRecordId} HTTPPut
            //Note: Id, CustomerId, ProductId, TimeStamp, DateCreated, & Quantity in  body
            //{ "Id":1,"CustomerId":1,"ProductId":33,"Quantity":2, "TimeStamp":"AAAAAAAA86s=", "DateCreated":"1/20/2016"}
            var cartRecord = await ShoppingCartTestHelpers.GetCartItem(_cartRecordId, ServiceAddress,RootAddress);
            cartRecord.Quantity = 5;
            var json = JsonConvert.SerializeObject(cartRecord);
            using (var client = new HttpClient())
            {
                var targetUrl = $"{ServiceAddress}{RootAddress}/{cartRecord.Id}";
                //throw new Exception($"{targetUrl}:{json}");
                var response = await client.PutAsync(targetUrl,
                    new StringContent(json,Encoding.UTF8, "application/json"));
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.StatusCode.ToString());
                }
                //Assert.True(response.IsSuccessStatusCode);
                Assert.Equal($"{ServiceAddress}api/shoppingcartrecord/{_customerId}".ToUpper(),
                    response.Headers.Location.AbsoluteUri.ToUpper());
            }
            // validate the cart item was updated
            var updatedCartRecord = await ShoppingCartTestHelpers.GetCartItem(_cartRecordId, ServiceAddress, RootAddress);
            Assert.Equal(5, updatedCartRecord.Quantity);
        }

        [Fact]
        public async void ShouldRemoveRecordOnUpdateIfQuantityBecomesZero()
        {
            var cartRecord = await ShoppingCartTestHelpers.GetCartItem(_cartRecordId, ServiceAddress, RootAddress);
            cartRecord.Quantity = 0;
            var json = JsonConvert.SerializeObject(cartRecord);
            using (var client = new HttpClient())
            {
                var targetUrl = $"{ServiceAddress}{RootAddress}/{cartRecord.Id}";
                var response = await client.PutAsync(targetUrl,
                    new StringContent(json, Encoding.UTF8, "application/json"));
                Assert.True(response.IsSuccessStatusCode);
                Assert.Equal($"{ServiceAddress}api/shoppingcartrecord/{_customerId}".ToUpper(),
                    response.Headers.Location.AbsoluteUri.ToUpper());
            }
            // validate the cart record was removed
            var cartItem = await ShoppingCartTestHelpers.GetCartItem(_cartRecordId, ServiceAddress, RootAddress);
            //Assert.Null(cartItem);
            Assert.Equal(0, cartItem.Id);
        }

        [Fact]
        public async void ShouldRemoveRecordOnUpdateIfQuantityBecomesLessThanZero()
        {
            var cartRecord = await ShoppingCartTestHelpers.GetCartItem(_cartRecordId, ServiceAddress, RootAddress);
            cartRecord.Quantity = -10;
            var json = JsonConvert.SerializeObject(cartRecord);
            using (var client = new HttpClient())
            {
                var targetUrl = $"{ServiceAddress}{RootAddress}/{cartRecord.Id}";
                var response = await client.PutAsync(targetUrl,
                    new StringContent(json, Encoding.UTF8, "application/json"));
                Assert.True(response.IsSuccessStatusCode);
                Assert.Equal($"{ServiceAddress}api/shoppingcartrecord/{_customerId}".ToUpper(),
                    response.Headers.Location.AbsoluteUri.ToUpper());
            }
            // validate the cart record was removed
            var cartItem = await ShoppingCartTestHelpers.GetCartItem(_cartRecordId, ServiceAddress, RootAddress);
            //Assert.Null(cartItem);
            Assert.Equal(0, cartItem.Id);
        }
    }
}
