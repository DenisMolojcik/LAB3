using laba3.Data;
using laba3.Infrastructure;
using laba3.Models;
using laba3.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace laba3
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string con = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<Context>(options => options.UseSqlServer(con));
            services.AddControllersWithViews();
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddScoped<ICachedDoctor, CachedDoctor>();
            services.AddScoped<ICachedTherapy, CachedTherapy>();
            services.AddScoped<SessionPatient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
            app.Map("/info", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    // Формирование строки для вывода 
                    string strResponse = "<HTML><HEAD><TITLE>Информация</TITLE><link rel=\"stylesheet\" href=\"css/site.css\"></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>Информация:</H1>";
                    strResponse += "<BR> Сервер: " + context.Request.Host;
                    strResponse += "<BR> Путь: " + context.Request.PathBase;
                    strResponse += "<BR> Протокол: " + context.Request.Protocol;
                    strResponse += "<BR><A href='/'>Главная</A></BODY></HTML>";
                    // Вывод данных
                    await context.Response.WriteAsync(strResponse);
                });
            });
            app.Map("/doctors", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    ICachedDoctor cachedDoctor = context.RequestServices.GetService<ICachedDoctor>();
                    IEnumerable<Doctor> doctors = cachedDoctor.GetList("doc15");
                    string HtmlString = "<HTML><HEAD><TITLE>Доктор</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>Список докторов</H1>" +
                    "<TABLE BORDER=1>";
                    HtmlString += "<TR>";
                    HtmlString += "<TH>Id</TH>";
                    HtmlString += "<TH>Имя доктора</TH>";
                    HtmlString += "<TH>Возраст</TH>";
                    HtmlString += "<TH>Пол</TH>";
                    HtmlString += "<TH>Долнжость</TH>";
                    HtmlString += "</TR>";
                    foreach (Doctor doctor in doctors)
                    {
                        HtmlString += "<TR>";
                        HtmlString += "<TD>" + doctor.DoctorId + "</TD>";
                        HtmlString += "<TD>" + doctor.Name + "</TD>";
                        HtmlString += "<TD>" + doctor.Gender + "</TD>";
                        HtmlString += "<TD>" + doctor.Position + "</TD>";
                        HtmlString += "</TR>";
                    }
                    HtmlString += "</TABLE>";
                    HtmlString += "<BR><A href='/'>Главная</A></BR>";
                    HtmlString += "</BODY></HTML>";
                    await context.Response.WriteAsync(HtmlString);
                });
            });
            app.Map("/therapies", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    ICachedTherapy cachedTherapies = context.RequestServices.GetService<ICachedTherapy>();
                    IEnumerable<Therapy> therapies = cachedTherapies.GetList("therap15");
                    string HtmlString = "<HTML><HEAD><TITLE>Терапия</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>Список терапий</H1>" +
                    "<TABLE BORDER=1>";
                    HtmlString += "<TR>";
                    HtmlString += "<TH>Id</TH>";
                    HtmlString += "<TH>Id средства</TH>";
                    HtmlString += "<TH>Id болезни</TH>";
                    HtmlString += "<TH>Id доктора</TH>";
                    HtmlString += "</TR>";
                    foreach (Therapy therapy in therapies)
                    {
                        HtmlString += "<TR>";
                        HtmlString += "<TD>" + therapy.Id + "</TD>";
                        HtmlString += "<TD>" + therapy.MedicianId + "</TD>";
                        HtmlString += "<TD>" + therapy.DiseaseId + "</TD>";
                        HtmlString += "<TD>" + therapy.DoctorId + "</TD>";
                        HtmlString += "</TR>";
                    }
                    HtmlString += "</TABLE>";
                    HtmlString += "<BR><A href='/'>Главная</A></BR>";
                    HtmlString += "</BODY></HTML>";
                    await context.Response.WriteAsync(HtmlString);
                });
            });
            app.Map("/patients", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    IEnumerable<Patient> patients = context.Session.Get<IEnumerable<Patient>>("patient");
                    string HtmlString = "<HTML><HEAD><TITLE>Пациент</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>Список пациентов</H1>" +
                    "<TABLE BORDER=1>";
                    HtmlString += "<TR>";
                    HtmlString += "<TH>Id</TH>";
                    HtmlString += "<TH>Фамилия</TH>";
                    HtmlString += "<TH>Имя</TH>";
                    HtmlString += "<TH>Отчество</TH>";
                    HtmlString += "<TH>Возраст</TH>";
                    HtmlString += "<TH>Пол</TH>";
                    HtmlString += "<TH>Телефон</TH>";
                    HtmlString += "<TH>Диагноз</TH>";
                    HtmlString += "</TR>";
                    foreach (Patient patient in patients)
                    {
                        HtmlString += "<TR>";
                        HtmlString += "<TD>" + patient.PatientId + "</TD>";
                        HtmlString += "<TD>" + patient.Surname + "</TD>";
                        HtmlString += "<TD>" + patient.Name + "</TD>";
                        HtmlString += "<TD>" + patient.Lastname + "</TD>";
                        HtmlString += "<TD>" + patient.Age + "</TD>";
                        HtmlString += "<TD>" + patient.Gender + "</TD>";
                        HtmlString += "<TD>" + patient.PhoneNumber + "</TD>";
                        HtmlString += "<TD>" + patient.Diagnos + "</TD>";
                        HtmlString += "</TR>";
                    }
                    HtmlString += "</TABLE>";
                    HtmlString += "<BR><A href='/'>Главная</A></BR>";
                    HtmlString += "</BODY></HTML>";
                    await context.Response.WriteAsync(HtmlString);
                });
            });
            app.Map("/searchByNameInDoctors", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    ICachedDoctor cachedDoctor = context.RequestServices.GetService<ICachedDoctor>();
                    IEnumerable<Doctor> doctors = cachedDoctor.GetList("doc15");
                    string doctorStr = "";
                    if (context.Request.Cookies.ContainsKey("Doc"))
                    {
                        doctorStr = context.Request.Cookies["Doc"];
                    }
                    string HtmlString = "<HTML><HEAD><TITLE>Врачи</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><FORM action ='/searchByNameInDoctors' / >" +
                    "Имя:<BR><INPUT type = 'text' name = 'DoctorBut' value = " + doctorStr + ">" +
                    "<BR><BR><INPUT type ='submit' value='Сохранить строку в куки'></FORM>" +
                    "<TABLE BORDER = 1>";
                    HtmlString += "<TR>";
                    HtmlString += "<TH>Id</TH>";
                    HtmlString += "<TH>Имя доктора</TH>";
                    HtmlString += "<TH>Возраст</TH>";
                    HtmlString += "<TH>Пол</TH>";
                    HtmlString += "<TH>Долнжость</TH>";
                    HtmlString += "</TR>";
                    doctorStr = context.Request.Query["DoctorBut"];
                    if (doctorStr != null)
                    {
                        context.Response.Cookies.Append("Doc", doctorStr);
                    }
                    foreach (Doctor doctor in doctors.Where(i => i.Name.Trim() == doctorStr))
                    {
                        HtmlString += "<TR>";
                        HtmlString += "<TD>" + doctor.DoctorId + "</TD>";
                        HtmlString += "<TD>" + doctor.Name + "</TD>";
                        HtmlString += "<TD>" + doctor.Gender + "</TD>";
                        HtmlString += "<TD>" + doctor.Position + "</TD>";
                        HtmlString += "</TR>";
                    }
                    HtmlString += "</TABLE>";
                    HtmlString += "<BR><A href='/'>Главная</A></BR>";
                    HtmlString += "</BODY></HTML>";
                    await context.Response.WriteAsync(HtmlString);
                });
            });
            app.Map("/searchByNameInPatients", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    IEnumerable<Patient> patients = context.Session.Get<IEnumerable<Patient>>("patient");
                    int patientAge = 20;
                    if (context.Session.Keys.Contains("age"))
                    {
                        patientAge = context.Session.Get<int>("age");
                    }
                    string HtmlString = "<HTML><HEAD><TITLE>Пациенты</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><FORM action ='/searchByNameInPatients' / >" +
                    "Имя:<BR><INPUT type = 'text' name = 'DoctorBut' value = " + patientAge + ">" +
                    "<BR><BR><INPUT type ='submit' value='Сохранить строку в сессию'></FORM>" +
                    "<TABLE BORDER = 1>";
                    HtmlString += "<TR>";
                    HtmlString += "<TH>Id</TH>";
                    HtmlString += "<TH>Фамилия</TH>";
                    HtmlString += "<TH>Имя</TH>";
                    HtmlString += "<TH>Отчество</TH>";
                    HtmlString += "<TH>Возраст</TH>";
                    HtmlString += "<TH>Пол</TH>";
                    HtmlString += "<TH>Телефон</TH>";
                    HtmlString += "<TH>Диагноз</TH>";
                    HtmlString += "</TR>";
                    patientAge = Convert.ToInt32(context.Request.Query["age"]);
                    foreach (Patient patient in patients.Where(i => i.Age > patientAge))
                    {
                        HtmlString += "<TR>";
                        HtmlString += "<TD>" + patient.PatientId + "</TD>";
                        HtmlString += "<TD>" + patient.Surname + "</TD>";
                        HtmlString += "<TD>" + patient.Name + "</TD>";
                        HtmlString += "<TD>" + patient.Lastname + "</TD>";
                        HtmlString += "<TD>" + patient.Age + "</TD>";
                        HtmlString += "<TD>" + patient.Gender + "</TD>";
                        HtmlString += "<TD>" + patient.PhoneNumber + "</TD>";
                        HtmlString += "<TD>" + patient.Diagnos + "</TD>";
                        HtmlString += "</TR>";
                    }
                    HtmlString += "</TABLE>";
                    HtmlString += "<BR><A href='/'>Главная</A></BR>";
                    HtmlString += "</BODY></HTML>";
                    await context.Response.WriteAsync(HtmlString);
                });
            });
            app.Run((context) =>
            {
                ICachedDoctor cachedCachedDoctor = context.RequestServices.GetService<ICachedDoctor>();
                cachedCachedDoctor.AddList("doc15");
                ICachedTherapy ICachedTherapy = context.RequestServices.GetService<ICachedTherapy>();
                ICachedTherapy.AddList("therap15");
                SessionPatient patients = context.RequestServices.GetService<SessionPatient>();
                context.Session.Set("patient", patients.GetList());
                string HtmlString = "<HTML><HEAD><TITLE>Больница</TITLE></HEAD>" +
                "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                "<BODY><H1>Главная</H1>";
                HtmlString += "<H2>Данные записаны в кэш сервера</H2>";
                HtmlString += "<BR><A href='/'>Главная</A></BR>";
                HtmlString += "<BR><A href='/searchByNameInDoctors'>Поиск по докторам</A></BR>";
                HtmlString += "<BR><A href='/searchByNameInPatients'>Поиск по пациентам</A></BR>";
                HtmlString += "<BR><A href='/doctors'>Док</A></BR>";
                HtmlString += "<BR><A href='/patients'>Пациенты</A></BR>";
                HtmlString += "<BR><A href='/therapies'>Терапии</A></BR>";
                HtmlString += "<BR><A href='/info'>Данные о сервере</A></BR>";
                HtmlString += "</BODY></HTML>";
                return context.Response.WriteAsync(HtmlString);
            });
        }
    }
}
