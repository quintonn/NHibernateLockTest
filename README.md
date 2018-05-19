# NHibernateLockTest
I created this project to track down an error I am receiving when using NHibernate with MS SQL


Just run the code as-is and it should error out on line 45 of Program.cs, which has this:

    using (var transaction = session.BeginTransaction())
    
First restore any nuget packages of course.
