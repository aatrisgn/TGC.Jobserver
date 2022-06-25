# TGC.Jobserver
This application is a personal project for creating a job/task server application. The purpose is, to programatically call operations for the application to, utilizing Hangfire as a queuing system for asynchronously processing.

# Introduction
This application seeks to support queuing and asynchronously processing for different types of applications via HTTP. The fundamental idea is to have C#-classes specified with specific behaviour for execution, which is 


## Purpose
The purpose of this application is to try and create a asynchronously server application which is able to recieve and execute jobs via HTTP-request.

# Current features

## HTTP-jobs
Generic HTTP-jobs can be defined in json and posted as request. Hangfire will then asynchronously process the request

## Callback
It is possible to define a callback URL which the application will make a POST-request to, at the end of the job. The request contains JobId and Http response status from defined request in job.

# Roadmap
This section describes what I would like to introduce, but have yet to have the time for. The sections are not prioritized, but will be updated once implemented.

## Better HTTP-Jobs
One of the few jobs which comes out-of-the-box is HTTP-jobs. At the moment, common HTTP-jobs has been implemented, but they are also lacking in terms of configurability. For instance, you are not able to specify authentication/authorization. 

## Dynamic localization of jobtypes
At the moment, each jobtype is manually injected into the IoC. This works, but ideally, it should be configurable, so you do not need a re-deployment for new jobtypes. However, this introduces security concerns for dynamic reading and executing code, and is also a bit tricky to do.

## Dependent jobs
At the moment, you are only able to create a single job and await of its defined exection. In the future, I would like to implement functionality for a process of jobs to be executed after each other.

## Custom job-queue/handling
At the moment, the solution is dependendent on Hangfire. This works fine for non-commercial personal projects, but if you would take part of this solution to an actual production environment, I am not sure, whether the license is valid (Not a legal kind of guy). Therefore, I would like to make the engine replaceable in the future. Currently, I have no idea how.

## Authorization in HTTP-jobs
At the moment, you are not able to make authorized calls. It is planned to allow for bearer-tokens and define headers.

## Expand on heartbeat
Currently, you can query the default .Net heartbeat at /health and a custom at api/heartbeat. This should be combined into a single controller with the relevant information.

# Known Issues
## Async jobs
At the moment, you are not able to define jobs which has a async execution method signature. Well, it is possible, but the error handling of the Hangfire exection does not detect it as being failed. It only works for synchronous methods.

I may have been doing the implementation wrong, but I was not able to detect failed jobs, which is problematic.

## Newtonsoft.Json vs. System.Text.Json
Hangfire is currently dependent on Newtonsoft.Json for serialization and deserialization. Since I went with System.Text.Json instead, there are some issues in regards to mismatches for the two libraries. This means, that you are not able to pass (some) System.Text.Json types to a job-definition, which is going to be handled by Hangfire.

I've yet to decide whether I should convert everything to Newtonsoft instead of System.Text.Json, but at the moment I am sticking with System.Text.Json. I do not suspect it to change in the future, unless more critical issues arise.

## Lack of Unit Tests
TBD (To-be-done)