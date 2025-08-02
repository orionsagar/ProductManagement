namespace ProductManagement.Infrastructure.Helpers
{
    public static class AppConfig
    {
        public static string ConnectionString = "Host=localhost;Port=5432;Database=ProductManagementDb;Username=postgres;Password=123";
            //"Server=localhost;Database=ProductManagementDb;User Id=sa;Password=yourStrong(!)Password;TrustServerCertificate=True;";

        public static string JwtIssuer = "ProductManagementIssuer";
        public static string JwtAudience = "ProductManagementAudience";
        public static string JwtKey= "ThisIsA32ByteLongSecretKeyValue1234";

        public static string JwtExpiration = "10080"; // in minutes

        public static string[] AllowedOrigins = new[]
        {
            "https://localhost:3000", // React app
            "http://localhost:3000"   // React app
        };
    }
}
