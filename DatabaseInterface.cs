using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;
using System.Collections;
using Dapper;

namespace nss.Data
{
    public class DatabaseInterface
    {
        public static SqliteConnection Connection
        {
            get
            {
                /*
                    Mac users: You can create an environment variable in your
                    .zshrc file.
                        export NSS_DB="/path/to/your/project/nss.db"

                    Windows users: You need to use a property window
                        http://www.forbeslindesay.co.uk/post/42833119552/permanently-set-environment-variables-on-windows
                 */
                string env = $"{Environment.GetEnvironmentVariable("NSS_DB")}";
                string _connectionString = $"Data Source={env}";
                return new SqliteConnection(_connectionString);
            }
        }


        public static void CheckCohortTable()
        {
            SqliteConnection db = DatabaseInterface.Connection;

            try
            {
                List<Cohort> cohorts = db.Query<Cohort>
                    ("SELECT Id FROM Cohort").ToList();
                    Console.WriteLine(cohorts);
            }
            catch (System.Exception ex)
            {
                if (ex.Message.Contains("no such table"))
                {
                    db.Execute(@"CREATE TABLE Cohort (
                        `Id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        `Name`	TEXT NOT NULL UNIQUE
                    )");

                    db.Execute(@"INSERT INTO Cohort
                        VALUES (null, 'Evening Cohort 1')");

                    db.Execute(@"INSERT INTO Cohort
                        VALUES (null, 'Day Cohort 10')");

                    db.Execute(@"INSERT INTO Cohort
                        VALUES (null, 'Day Cohort 11')");

                    db.Execute(@"INSERT INTO Cohort
                        VALUES (null, 'Day Cohort 12')");

                    db.Execute(@"INSERT INTO Cohort
                        VALUES (null, 'Day Cohort 13')");

                    db.Execute(@"INSERT INTO Cohort
                        VALUES (null, 'Day Cohort 21')");

                }
            }
        }
// CREATING OF EXERCISE TABLE AND SEEDING
        public static void CheckExerciseTable()
        {
            SqliteConnection db = DatabaseInterface.Connection;

            try
            {
                List<Exercise> exercises = db.Query<Exercise>
                    ("SELECT Id FROM Exercise").ToList();
            }
            catch (System.Exception ex)
            {
                if (ex.Message.Contains("no such table"))
                {
                    db.Execute(@"CREATE TABLE Exercise (
                        `Id`	    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        `Name`	    TEXT NOT NULL, 
                        `Language`  TEXT NOT NULL
                    )");

                    db.Execute(@"INSERT INTO Exercise
                        VALUES (null, 'ChickenMonkey', 'JavaScript')");

                    db.Execute(@"INSERT INTO Exercise
                        VALUES (null, 'BattleBands', 'JavaScript')");

                    db.Execute(@"INSERT INTO Exercise
                        VALUES (null, 'Kennel', 'React')");

                    db.Execute(@"INSERT INTO Exercise
                        VALUES (null, 'MusicHistory', 'SQL')");

                    db.Execute(@"INSERT INTO Exercise
                        VALUES (null, 'NutShell', 'JavaScript')");

                    db.Execute(@"INSERT INTO Exercise
                        VALUES (null, 'ReactiveNutshell', 'React')");

                }
            }
        }

        public static void CheckInstructorsTable()
        {
            SqliteConnection db = DatabaseInterface.Connection;

            try
            {
                List<Instructor> toys = db.Query<Instructor>
                    ("SELECT Id FROM Instructor").ToList();
            }
            catch (System.Exception ex)
            {
                if (ex.Message.Contains("no such table"))
                {
                    db.Execute($@"CREATE TABLE Instructor (
                        `Id`	integer NOT NULL PRIMARY KEY AUTOINCREMENT,
                        `FirstName`	varchar(80) NOT NULL,
                        `LastName`	varchar(80) NOT NULL,
                        `SlackHandle`	varchar(80) NOT NULL,
                        `Specialty` varchar(80),
                        `CohortId`	integer NOT NULL,
                        FOREIGN KEY(`CohortId`) REFERENCES `Cohort`(`Id`)
                    )");

                    db.Execute($@"INSERT INTO Instructor
                        SELECT null,
                              'Steve',
                              'Brownlee',
                              '@coach',
                              'Dad jokes',
                              c.Id
                        FROM Cohort c WHERE c.Name = 'Evening Cohort 1'
                    ");

                    db.Execute($@"INSERT INTO Instructor
                        SELECT null,
                              'Joe',
                              'Shepherd',
                              '@joes',
                              'Analogies',
                              c.Id
                        FROM Cohort c WHERE c.Name = 'Day Cohort 13'
                    ");

                    db.Execute($@"INSERT INTO Instructor
                        SELECT null,
                              'Jisie',
                              'David',
                              '@jisie',
                              'Student success',
                              c.Id
                        FROM Cohort c WHERE c.Name = 'Day Cohort 21'
                    ");
                }
            }
        }
// CREATING OF STUDENT TABLE AND SEEDING WITH SUB-SELECTS
        public static void CheckStudentTable()
        {
            SqliteConnection db = DatabaseInterface.Connection;

            try
            {
                List<Student> students = db.Query<Student>
                    ("SELECT Id FROM Student").ToList();
            }
            catch (System.Exception ex)
            {
                if (ex.Message.Contains("no such table"))
                {
                    db.Execute($@"CREATE TABLE Student (
                        `Id`	integer NOT NULL PRIMARY KEY AUTOINCREMENT,
                        `FirstName`	varchar(80) NOT NULL,
                        `LastName`	varchar(80) NOT NULL,
                        `SlackHandle`	varchar(80) NOT NULL,
                        `CohortId`	integer NOT NULL,
                        FOREIGN KEY(`CohortId`) REFERENCES `Cohort`(`Id`)
                    )");

                    db.Execute($@"INSERT INTO Student
                        SELECT null,
                              'Lauren',
                              'Richert',
                              '@lrichert',
                              c.Id
                        FROM Cohort c WHERE c.Name = 'Day Cohort 13'
                    ");

                    db.Execute($@"INSERT INTO Student
                        SELECT null,
                              'Carmen',
                              'Boyd',
                              '@cboyd',
                              c.Id
                        FROM Cohort c WHERE c.Name = 'Evening Cohort 1'
                    ");

                    db.Execute($@"INSERT INTO Student
                        SELECT null,
                              'Sam',
                              'Jones',
                              '@sjones',
                              c.Id
                        FROM Cohort c WHERE c.Name = 'Day Cohort 21'
                    ");
                }
            }
        }
    // CREATE STUDENTEXERCISE TABLE AND SEED IT
            public static void CheckStudentExerciseTable()
        {
            SqliteConnection db = DatabaseInterface.Connection;

            try
            {
                List<StudentExercise> studentExercises = db.Query<StudentExercise>
                    ("SELECT Id FROM StudentExercise").ToList();
            }
            catch (System.Exception ex)
            {
                if (ex.Message.Contains("no such table"))
                {
                    db.Execute($@"CREATE TABLE StudentExercise (
                        `Id`	        integer NOT NULL PRIMARY KEY AUTOINCREMENT,
                        `ExerciseId`	integer NOT NULL,
                        `StudentId`	    integer NOT NULL,
                        `InstructorId`	integer NOT NULL,
                        FOREIGN KEY(`ExerciseId`) REFERENCES `Exercise`(`Id`)
                        FOREIGN KEY(`StudentId`) REFERENCES `Student`(`Id`)
                        FOREIGN KEY(`InstructorId`) REFERENCES `InstructorId`(`Id`)
                    )");

            db.Execute($@"INSERT INTO StudentExercise
                SELECT null, e.Id, s.Id, i.Id
                FROM Student s, Exercise e, Instructor i
                WHERE e.Name = 'Overly Excited'
                AND s.SlackHandle = '@ryan.tanay'
                AND i.SlackHandle = '@coach'
            ");


            db.Execute($@"INSERT INTO StudentExercise
                SELECT null, e.Id, s.Id, i.Id
                FROM Student s, Exercise e, Instructor i
                WHERE e.Name = 'Overly Excited'
                AND s.SlackHandle = '@katerebekah'
                AND i.SlackHandle = '@coach'
            ");


            db.Execute($@"INSERT INTO StudentExercise
                SELECT null, e.Id, s.Id, i.Id
                FROM Student s, Exercise e, Instructor i
                WHERE e.Name = 'ChickenMonkey'
                AND s.SlackHandle = '@juanrod'
                AND i.SlackHandle = '@joes'
            ");


            db.Execute($@"INSERT INTO StudentExercise
                SELECT null, e.Id, s.Id, i.Id
                FROM Student s, Exercise e, Instructor i
                WHERE e.Name = 'Boy Bands & Vegetables'
                AND s.SlackHandle = '@katerebekah'
                AND i.SlackHandle = '@jisie'
            ");
        }
                }
            }
        }
        }
    
