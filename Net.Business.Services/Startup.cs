using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Net.Business.Services
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IServiceProvider ServiceProvider { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();
            services.ConfigureIISIntegration();
            services.ConfigureSQLConnection();
            services.ConfigureHttpClientServiceLayer();

            // Para obtener datos de los header
            services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();

            //Autenticacion
            string semilla = Configuration.GetSection("ParametrosTokenConfig").GetValue<string>("Semilla");
            string emisor = Configuration.GetSection("ParametrosTokenConfig").GetValue<string>("Emisor");
            string destinatario = Configuration.GetSection("ParametrosTokenConfig").GetValue<string>("Destinatario");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(semilla));

            services.AddAuthentication
                (JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt => {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = emisor,
                        ValidAudience = destinatario,
                        IssuerSigningKey = key
                    };

                });
            services.ConfigureRepositoryWrapper();

            /*De aqui en adelante configuracion de documentacion de nuestra API*/
            services.AddSwaggerGen(options => {
                options.SwaggerDoc("ApiAtencion", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "API General Atención",
                    Version = "1",
                    Description = "BackEnd Atención",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "luis.tasayco@sba.pe",
                        Name = "Grupo SBA",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    }
                });

                options.SwaggerDoc("ApiCentro", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "API General Centro",
                    Version = "1",
                    Description = "BackEnd Centro",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "luis.tasayco@sba.pe",
                        Name = "Grupo SBA",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    }
                });

                options.SwaggerDoc("ApiCliente", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "API General Cliente",
                    Version = "1",
                    Description = "BackEnd Cliente",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "luis.tasayco@sba.pe",
                        Name = "Grupo SBA",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    }
                });

                options.SwaggerDoc("ApiVenta", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "API General Ventas",
                    Version = "1",
                    Description = "BackEnd Ventas",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "luis.tasayco@sba.pe",
                        Name = "Grupo SBA",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    }
                });

                options.SwaggerDoc("ApiComprobante", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "API General Comprobante",
                    Version = "1",
                    Description = "BackEnd Comprobante",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "luis.tasayco@sba.pe",
                        Name = "Grupo SBA",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    }
                });

                options.SwaggerDoc("ApiHospital", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "API General Hospital",
                    Version = "1",
                    Description = "BackEnd Hospital",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "luis.tasayco@sba.pe",
                        Name = "Grupo SBA",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    }
                });

                options.SwaggerDoc("ApiMedico", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "API General Médico",
                    Version = "1",
                    Description = "BackEnd Médico",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "luis.tasayco@sba.pe",
                        Name = "Grupo SBA",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    }
                });

                options.SwaggerDoc("ApiPaciente", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "API General Paciente",
                    Version = "1",
                    Description = "BackEnd Paciente",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "luis.tasayco@sba.pe",
                        Name = "Grupo SBA",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    }
                });

                options.SwaggerDoc("ApiPedido", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "API General Pedido",
                    Version = "1",
                    Description = "BackEnd Pedido",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "luis.tasayco@sba.pe",
                        Name = "Grupo SBA",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    }
                });

                options.SwaggerDoc("ApiPersonaClinica", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "API General Persona Clínica",
                    Version = "1",
                    Description = "BackEnd Persona Clínica",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "luis.tasayco@sba.pe",
                        Name = "Grupo SBA",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    }
                });

                options.SwaggerDoc("ApiPlanes", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "API General Planes",
                    Version = "1",
                    Description = "BackEnd Planes",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "luis.tasayco@sba.pe",
                        Name = "Grupo SBA",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    }
                });

                options.SwaggerDoc("ApiProducto", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "API General Producto",
                    Version = "1",
                    Description = "BackEnd Producto",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "luis.tasayco@sba.pe",
                        Name = "Grupo SBA",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    }
                });

                options.SwaggerDoc("ApiReceta", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "API General Receta",
                    Version = "1",
                    Description = "BackEnd Receta",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "luis.tasayco@sba.pe",
                        Name = "Grupo SBA",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    }
                });

                options.SwaggerDoc("ApiSerie", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "API General Serie",
                    Version = "1",
                    Description = "BackEnd Serie",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "luis.tasayco@sba.pe",
                        Name = "Grupo SBA",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    }
                });

                options.SwaggerDoc("ApiSeriePorMaquina", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "API General Serie Por Maquina",
                    Version = "1",
                    Description = "BackEnd Serie Por Maquina",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "luis.tasayco@sba.pe",
                        Name = "Grupo SBA",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    }
                });

                options.SwaggerDoc("ApiTabla", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "API General Tabla",
                    Version = "1",
                    Description = "BackEnd Tabla",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "luis.tasayco@sba.pe",
                        Name = "Grupo SBA",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    }
                });

                options.SwaggerDoc("ApiTipoCambio", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "API General Tipo Cambio",
                    Version = "1",
                    Description = "BackEnd Tipo Cambio",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "luis.tasayco@sba.pe",
                        Name = "Grupo SBA",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    }
                });

                options.SwaggerDoc("ApiAlmacen", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "API General Almacén",
                    Version = "1",
                    Description = "BackEnd Almacén",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "luis.tasayco@sba.pe",
                        Name = "Grupo SBA",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    }
                });

                options.SwaggerDoc("ApiApu", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "API - Proveedor APU",
                    Version = "1",
                    Description = "BackEnd - Para el proveedor APU",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "luis.tasayco@sba.pe",
                        Name = "Grupo SBA",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://www.linkedin.com/in/luis-miguel-tasayco-marquez/")
                    }
                });

                var archivoXmlComentarios = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var rutaApiComentarios = Path.Combine(AppContext.BaseDirectory, archivoXmlComentarios);

                options.IncludeXmlComments(rutaApiComentarios);

                /*Primero definir el esquema de seguridad*/
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "Autenticacion JWT (Bearer)",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Id = "Bearer",
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme
                            }
                        }, new List<string>()
                    }
                });
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.ConfigureExceptionHandler();

            //Linea para documentacion api
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/ApiAtencion/swagger.json", "API General Atención");
                options.SwaggerEndpoint("/swagger/ApiCentro/swagger.json", "API General Centro");
                options.SwaggerEndpoint("/swagger/ApiCliente/swagger.json", "API General Cliente");
                options.SwaggerEndpoint("/swagger/ApiVenta/swagger.json", "API General Ventas");
                options.SwaggerEndpoint("/swagger/ApiComprobante/swagger.json", "API General Comprobante");
                options.SwaggerEndpoint("/swagger/ApiHospital/swagger.json", "API General Hospital");
                options.SwaggerEndpoint("/swagger/ApiMedico/swagger.json", "API General Médico");
                options.SwaggerEndpoint("/swagger/ApiPaciente/swagger.json", "API General Paciente");
                options.SwaggerEndpoint("/swagger/ApiPedido/swagger.json", "API General Pedido");
                options.SwaggerEndpoint("/swagger/ApiPersonaClinica/swagger.json", "API General Persona Clínica");
                options.SwaggerEndpoint("/swagger/ApiPlanes/swagger.json", "API General Planes");
                options.SwaggerEndpoint("/swagger/ApiProducto/swagger.json", "API General Producto");
                options.SwaggerEndpoint("/swagger/ApiReceta/swagger.json", "API General Receta");
                options.SwaggerEndpoint("/swagger/ApiSerie/swagger.json", "API General Serie");
                options.SwaggerEndpoint("/swagger/ApiSeriePorMaquina/swagger.json", "API General Serie Por Maquina");
                options.SwaggerEndpoint("/swagger/ApiTabla/swagger.json", "API General Tabla");
                options.SwaggerEndpoint("/swagger/ApiTipoCambio/swagger.json", "API General Tipo Cambio");
                options.SwaggerEndpoint("/swagger/ApiAlmacen/swagger.json", "API General Almacén");
                options.SwaggerEndpoint("/swagger/ApiApu/swagger.json", "API - Proveedor APU");
                options.RoutePrefix = "";
            });

            app.UseRouting();

            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
