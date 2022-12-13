<a name="readme-top"></a>
# Online-Learning-Website-with-Chatbot-Correction-Pages
<br />
<div align="center">
  <img src="https://user-images.githubusercontent.com/52370596/200297466-9aebdc87-524a-4d70-8a1c-628e97b2cc29.png" width="600" height="auto">
   

  <h3 align="center"><a href="https://dsta.azurewebsites.net/"><strong>Learning System with Chatbot Correction Page</strong></a></h3>

  <p align="center">
    <strong>Log in ID : visitor </strong>
    <br>
      <a href="https://dsta.azurewebsites.net/"><strong>Explore the website »</strong></a>
  </p>
</div>

<details>
    <summary>Table of Contents</summary>
    <ol>
      <li>
        <a href="#about-the-project">About The Project</a>
        <ul>
          <li><a href="#how-to-use">How to Use</a></li>
        </ul>
        <ul>
          <li><a href="#built-with">Built With</a></li>
        </ul>
      </li>
      <li>
        <a href="#demo">Demo</a>
        <ul>
          <li><a href="#login">Login</a></li>
          <li><a href="#clear-button">Clear Button</a></li>
        </ul>
      </li>
      <li>
        <a href="#about-me">About Me</a>
        <ul>
          <li><a href="#how-it-was-created">How it was Created</a></li>
          <li><a href="#contact">Contact</a></li>
        </ul>
      </li>
    </ol>
  </details>
  
<!-- ABOUT THE PROJECT -->
## About The Project
Hi, this is a demo version of the learning system mentioned in my <b><a href="https://doi.org/10.1007/s10639-022-11506-6">master's thesis</a></b>. <br/>
It's an Asp.Net MVC5 website for students to learn computer programs and correct wrong answers with chatbots. <br>
The example units are heap and heapsort algorithms.

### How to Use
<img src="https://user-images.githubusercontent.com/52370596/200298988-99440d78-2839-46e3-83b1-95bf0b6db6f0.png" width="800" height="auto">
1. Choose any unit on the home page, and start the lesson.<br>
2. In each unit, the user will watch the tutorial and then answer a question related to the video. If the user gets the answer wrong, he or she will be directed to the chatbot page for correction, then the system will lead the user to answer a similar question again.<br>
3. The unit review page will unlock the units finished by the user.<br>
4. The learning status page explains completion progress with timelines.

### Built With
The learning system was built with Asp.net MVC5 framework and connects to a SQL-Server implemented on an Azure VM and the chatbots were created with Dialogflow. <br>
I published the demo version to Azure after the experiment.
* [![Asp.net MVC5][MVC5_badge]][MVC5_url]
* [![SQL Server][sql_badge]][sql_url]
* [![Azure][azure_badge]][azure_url]
* [![dialogflow][dialogflow_badge]][dialogflow_url]

<p align="right">(<a href="#readme-top">back to top</a>)</p>
<!-- ABOUT THE PROJECT -->

<!-- Demo -->
## Demo 
<b><a href="https://dsta.azurewebsites.net">Go to Demo >></a></b><br>
### Login
<img src="https://user-images.githubusercontent.com/52370596/200781762-5c6f3337-49a8-48df-a183-7f23601cb09a.png" width="500" height="auto">
Type "visitor" to log in, then click the green "yes" button. <br>
It's 99% the same as the one used in the experiment, except for a few minor updates on the presentation.

### Clear Button
<img src="https://user-images.githubusercontent.com/52370596/202126139-dc923653-a4af-451e-924e-786f81aa7d13.png" width="800" height="auto">
Scroll to the bottom of the home page, and there's a clear button. Click to clear all learning progress if all the exercises were completed on this account. Otherwise, the tutorial functions will be locked for preventing the students to rewrite their homework.<br>
This feature only exists on demo version, it's not available to students.

<p align="right">(<a href="#readme-top">back to top</a>)</p>
<!-- Demo -->

<!-- ABOUT ME -->
## About Me
### How it was created
I learned Asp.Net MVC5 at UCOM then self-learned HTML, CSS, and some related Azure functions. SQL server and Dialogflow were first introduced at school but googled and learned a lot while finishing this project.

### Contact Me
Chen, Yun-an - vivian0602.chen@gmail.com

<p align="right">(<a href="#readme-top">back to top</a>)</p>
<!-- ABOUT ME -->


<!-- MARKDOWN LINKS & IMAGES -->
[MVC5_badge]: https://img.shields.io/badge/-Asp.net%20MVC5-f2f2f2?logo=.NET&logoColor=512BD4&style=for-the-badge
[MVC5_url]: https://dotnet.microsoft.com/en-us/apps/aspnet/mvc

[sql_badge]: https://img.shields.io/badge/-SQL%20Server-cccccc?logo=Microsoft-SQL-Server&logoColor=CC2927&style=for-the-badge
[sql_url]: https://www.microsoft.com/en-us/sql-server/sql-server-downloads

[azure_badge]: https://img.shields.io/badge/-Azure-b3b3b3?logo=Microsoft-Azure&logoColor=0078D4&style=for-the-badge
[azure_url]: https://azure.microsoft.com/en-us/

[dialogflow_badge]: https://img.shields.io/badge/-Dialogflow-999999?logo=Dialogflow&logoColor=FF9800&style=for-the-badge
[dialogflow_url]: https://cloud.google.com/dialogflow
