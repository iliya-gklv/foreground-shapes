# foreground-shapes. 
.NET application that can recognize foreground shapes (in physical order). 

There are set of geomerical shapes (i.e. lines, rectangles, circles and triangles) which are rendered in the physical order. 
•	Line is defined by pair of coordinates;  
•	Rectangle is defined by top, left corner, width and height;  
•	Circle is defined by the center and radius;  
•	Triangle is defined by 3 points.  
You should write .NET API (dll) to recognize foreground shapes (shapes which are over any others).    
Example of the solution provided.   
  <img width="653" alt="image" src="https://user-images.githubusercontent.com/119431174/204573949-eb51fd66-4ae1-4458-bd16-800e8e8ab950.png">. 

Quality requirements:  
Q1 You should find n (or all) foreground shapes, which meet threshold (minimal area) .  
Q2 Algorithm should be optimized by the speed and memory usage;  
Q3 Algorithm should allow simple extension by other types of shapes;  
Q4 Algorithm should work synchronously and asynchronously. Working asynchronously results. 
should be available “on the fly”, demonstrating the foreground shapes as soon as they are. 
recognized;  
Q5 Your API shoud be thread-safe and that should be proven by the tests. 
Q6 Unit tests should be developed, using nunit with good coverage;  
Q7 Don&#39;t hesitate to create generator for set of shapes to have the examples for testing;  
Q8 Please, provide read me file, describing your design approach, strong and week features, API. 
reference, possible ways to impove.  
Q9 Your API implementation and QA criteria satisfaction should be prooven by the simple. 
runnable example of the API usage, and well designed source code.  
Constraints :  
C1 You should implement 100% native .NET/C# solution, avoiding usage of third party libraries.  

# Result.

<img width="603" alt="image" src="https://user-images.githubusercontent.com/119431174/204573501-32cf6f63-4bbd-4878-882d-80ae63b6ec35.png">

