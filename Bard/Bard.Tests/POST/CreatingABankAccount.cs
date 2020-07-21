﻿using Bard;
using Bard.Configuration;
using Fluent.Testing.Sample.Api;
using Fluent.Testing.Sample.Api.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Xunit;
using Xunit.Abstractions;

namespace Fluent.Testing.Library.Tests.POST
{
    public class CreatingABankAccount
    {
        public CreatingABankAccount(ITestOutputHelper output)
        {
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(builder =>
                    builder
                        .UseStartup<Startup>()
                        .UseTestServer()
                        .UseEnvironment("development"));

            var host = hostBuilder.Start();

            var httpClient = host.GetTestClient();

            Scenario = ScenarioConfiguration
                .Configure(options =>
                {
                    options.UseHttpClient(httpClient);
                    options.Log(output.WriteLine);
                });

            When = Scenario.When;
            Then = Scenario.Then;
        }

        private const string ApiBankaccounts = "api/bankaccounts";

        public IWhen When { get; set; }
        public IThen Then { get; set; }

        public IFluentScenario Scenario { get; set; }

        [Fact]
        public void When_creating_a_bank_account()
        {
            When
                .Post(ApiBankaccounts, new BankAccount
                {
                    CustomerName = "Ranulph Fiennes"
                });

            Then
                .Response
                .ShouldBe
                .Created();
        }
    }
}