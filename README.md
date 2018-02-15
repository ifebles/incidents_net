# incidents_net

To start the project:

1. Go to the ./Incidents folder and execute "docker build -t test/incidents ."

2. When finished building, execute "docker run -d -p 8000:80 test/incidents"

3. Go to your browser and navigate to "http://localhost:8000/checkincidents/incidents" to see it working!



PS: No actual database connection were added but a local static data usage instead due to EF (Entity Framework) facility to handle collections. With an actual Database connection there is only one thing required and that's the connection, but it's quite the same methodology.
