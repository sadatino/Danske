Setup docker:
  1. cd Danske
  2. docker compose up --build
  3. [http://localhost:8080/swagger](http://localhost:8080/swagger/index.html)
Or launch in an IDE

Workflow:
Through swagger, every endpoint has a description and a summary

Assumptions:
1. business rules:
   
  a. Daily tax can be any day of the year

  b. Weekly can be any week of the year
  
  c. Monthly needs to start on the first day of the month, can`t have two monthly taxes on the same month
  
  d. Yearly - starts on January first, only one per year
