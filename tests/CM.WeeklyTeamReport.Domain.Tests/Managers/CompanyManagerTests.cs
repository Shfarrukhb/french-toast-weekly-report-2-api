﻿using CM.WeeklyTeamReport.Domain.Commands;
using CM.WeeklyTeamReport.Domain.Entities.Implementations;
using CM.WeeklyTeamReport.Domain.Entities.Interfaces;
using CM.WeeklyTeamReport.Domain.Repositories.Dto;
using CM.WeeklyTeamReport.Domain.Repositories.Interfaces;
using CM.WeeklyTeamReport.Domain.Repositories.Managers;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace CM.WeeklyTeamReport.Domain.Tests
{
    public class CompanyManagerTests
    {
        [Fact]
        public void ShouldReadAllCompanies()
        {
            var fixture = new CompanyManagerFixture();
            fixture.CompanyRepository.Setup(x => x.ReadAll()).Returns(
                new List<ICompany>() {
                    new Company { Name = "Trevor Philips Industries", ID=1 },
                    new Company { Name = "Aperture Science", ID=2 }
                });
            var manager = fixture.GetCompanyManager();

            var companies = (List <CompanyDto>)manager.readAll();
            companies.Should().HaveCount(2);
            companies[0].ID.Should().Be(1);
            companies[0].Name.Should().Be("Trevor Philips Industries");
            companies[1].ID.Should().Be(2);
            companies[1].Name.Should().Be("Aperture Science");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        public void ShouldReadCompanyByID(int id)
        {
            var fixture = new CompanyManagerFixture();

            fixture.CompanyRepository.Setup(x => x.Read(id)).Returns(               
                    new Company { Name = "Trevor Philips Industries", ID= id });
            var manager = fixture.GetCompanyManager();

            var company = manager.read(id);
            company.Should().NotBeNull();
            company.Should().BeOfType<CompanyDto>();
            company.ID.Should().Be(id);
            company.Name.Should().Be("Trevor Philips Industries");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        public void ShouldDeleteCompanyByID(int id)
        {
            var fixture = new CompanyManagerFixture();
            var company = new Company { Name = "Trevor Philips Industries", ID = id };
            fixture.CompanyRepository.Setup(x => x.Delete(id));
            var manager = fixture.GetCompanyManager();

            manager.delete(id);
            fixture.CompanyRepository.Verify(x => x.Delete(id), Times.Once);
        }
      

        public class CompanyManagerFixture
        {
            public CompanyManagerFixture()
            {
                CompanyRepository = new Mock<ICompanyRepository>();
            }

            public Mock<ICompanyRepository> CompanyRepository { get; private set; }

            public CompanyManager GetCompanyManager()
            {
                return new CompanyManager(CompanyRepository.Object);
            }
        }
    }
}
