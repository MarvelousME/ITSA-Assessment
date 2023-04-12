# ITSA-Assessment
Assessment for ITSA

# Instructions

1. Change the database connection string to point to your own database, this is found in the appsettings.json file.<br/>
2. I have created the database migrations, you will need to run the 'update-database' command in the Package Manager Console.<br/>
3. Set the VetClinic.Api as your startup project, then you run it to seed the database with dummy and static data. <br/>
4. Swagger interface will then show and you can test the Api method calls.<br/>
5. Please not that I have enabled oAuth2 Authentication on the controllers so you will need to log in with the default admin user and select the scope.<br/>
   This will store claims in a JWT token that is passed in a request which is authenticated and a refresh token will be issued if the token expired<br/>
   Admin User<br/>
   ----------
   Username: admin
   Password: Admin@123
