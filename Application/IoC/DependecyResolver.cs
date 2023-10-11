using Application.AutoMapper;
using Application.Services.Abstract;
using Application.Services.Concrete;
using Autofac;
using AutoMapper;
using Domain.Repositories;
using Infrastructer.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IoC
{
    public class DependecyResolver : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AppUserService>().As<IAppUserService>().InstancePerLifetimeScope();
            builder.RegisterType<AppUserRepository>().As<IAppUserRepository>().InstancePerLifetimeScope();

            builder.RegisterType<DepartmentService>().As<IDepartmentService>().InstancePerLifetimeScope();
            builder.RegisterType<DepartmentRepository>().As<IDepartmentRepository>().InstancePerLifetimeScope();

            builder.RegisterType<CompanyService>().As<ICompanyService>().InstancePerLifetimeScope();
            builder.RegisterType<CompanyRepository>().As<ICompanyRepository>().InstancePerLifetimeScope();

            builder.RegisterType<CompanyManagerService>().As<ICompanyManagerService>().InstancePerLifetimeScope();

            builder.RegisterType<EmployeeService>().As<IEmployeeService>().InstancePerLifetimeScope();

            builder.RegisterType<LeaveService>().As<ILeaveService>().InstancePerLifetimeScope();
            builder.RegisterType<LeaveRepository>().As<ILeaveRepository>().InstancePerLifetimeScope();

            builder.RegisterType<LeaveAppUserService>().As<ILeaveAppUserService>().InstancePerLifetimeScope();
            builder.RegisterType<LeaveAppUserRepository>().As<ILeaveAppUserRepository>().InstancePerLifetimeScope();

            builder.Register(context => new MapperConfiguration(cfg =>
            {
                //Register Mapper Profile
                cfg.AddProfile<ModelMapper>();
            }
           )).AsSelf().SingleInstance();

            builder.Register(c =>
            {
                //This resolves a new context that can be used later.
                var context = c.Resolve<IComponentContext>();
                var config = context.Resolve<MapperConfiguration>();
                return config.CreateMapper(context.Resolve);
            })
            .As<IMapper>()
            .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
