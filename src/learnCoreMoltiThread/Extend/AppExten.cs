using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;

namespace Microsoft.AspNetCore.Builder
{
    public static class AppExten
    {
        public static IApplicationBuilder UseNodeModules(this IApplicationBuilder app, string root)
        {
            var path = Path.Combine(root, "node_modules");
            var provider = new PhysicalFileProvider(path);
            var option = new StaticFileOptions();
            option.RequestPath = "/node_modules";
            option.FileProvider = provider;
            app.UseStaticFiles(option);
            return app;
        }
    }
}

