using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using FileHelpers;
using System.Collections.Generic;
using System.Configuration;
using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace helper_utility
{
    enum Operation
    {
        //Get all projects (with all details) in an Azure DevOps Orgnizaton
        GetProjects,
        //Get all repos (with all details) in an Azure DevOps Orgnizaton
        GetRepos,
        //Get all build pipelines (with all details) in an Azure DevOps Orgnizaton
        GetBuildPipelines,
        //Get all release pipelines (with all details) in an Azure DevOps Orgnizaton
        GetReleasePipelines,
        //Get all variable groups (with all details) in an Azure DevOps Orgnizaton
        GetAllVariableGroups,
        //Get all task groups (with all details) in an Azure DevOps Orgnizaton
        GetAllTaskGroups,
        //Get all users (with all details) in an Azure DevOps Orgnizaton
        GetAllUsers,
        //Get all users from a specific(with all details) in an Azure DevOps Porject
        GetAllUsersFromProjectGroups,
        //Remove all users from a specific in an Azure DevOps Porject
        RemoveAllUsersFromProjectGroups,
        //Move variable group from one Azure DevOps porject to another
        MigrateProjectVariableGroups
    }
    class Program
    {
        static void Main(string[] args)
        {
            ILog logger = null;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());

            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            logger = LogManager.GetLogger(typeof(Program));

            using (var client = new HttpClient())
            {
                
                ConsoleLogger Logger = new ConsoleLogger();
                string personalaccesstoken = ConfigurationManager.AppSettings["personalAccessToken"];
                
                var credentials = Convert.ToBase64String(
                       System.Text.ASCIIEncoding.ASCII.GetBytes(
                           string.Format("{0}:{1}", "", personalaccesstoken)));
                
                string azure_devops_Url = "https://dev.azure.com/" + ConfigurationManager.AppSettings["orgName"] + "/";
                client.BaseAddress = new Uri(azure_devops_Url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                //int caseSwitch = 3;
                string opsString = ConfigurationManager.AppSettings["operation"];
                Operation opsValue = (Operation)Enum.Parse(typeof(Operation), opsString);

                switch (opsValue)
                {
                    case Operation.GetProjects:
                        Logger.Message("function GetProjects called", logger);
                        GetProjects(client, Logger, logger);                        
                        break;
                    case Operation.GetRepos:
                        Logger.Message("function GetRepos called", logger);
                        GetRepos(client, Logger, logger);                       
                        break;
                    case Operation.GetBuildPipelines:
                        Logger.Message("function GetBuildPipelines called", logger);
                        GetBuildPipelines(client, Logger, logger);                       
                        break;
                    case Operation.GetReleasePipelines:
                        Logger.Message("function GetReleasePipelines called", logger);
                        GetReleasePipelines(client, Logger, logger);                        
                        break;
                    case Operation.GetAllUsers:
                        Logger.Message("function GetAllUsers called", logger);
                        GetAllUsers(client, Logger, logger);                        
                        break;
                    case Operation.GetAllUsersFromProjectGroups:
                        Logger.Message("function GetAllUsersFromProjectGroups called", logger);
                        GetAllUsersFromProjectGroups(Logger, logger);
                        break;
                    case Operation.GetAllVariableGroups:
                        Logger.Message("function GetAllVariableGroups called", logger);
                        GetAllVariableGroups(client, Logger, logger);
                        break;
                    case Operation.GetAllTaskGroups:
                        Logger.Message("function GetAllTaskGroups called", logger);
                        GetAllTaskGroups(client, Logger, logger);
                        break;
                    case Operation.RemoveAllUsersFromProjectGroups:
                        Logger.Message("function RemoveAllUsersFromProjectGroups called", logger);
                        RemoveAllUsersFromProjectGroups(Logger, logger);
                        break;
                    case Operation.MigrateProjectVariableGroups:
                        Logger.Message("function MigrateProjectVariableGroups called", logger);
                        MigrateProjectVariableGroups(client, Logger, logger);
                        break;
                    default:
                        Logger.StatusBegin("Default case", logger);
                        break;
                }
                 
            }
            
        }

        static public void GetProjects(HttpClient client, ConsoleLogger Logger, ILog fileLogger)
        {
            string responseString = null;
            HttpResponseMessage response = client.GetAsync("_apis/projects?$top=250&stateFilter=All&api-version=1.0").Result;


            if (response != null && response.IsSuccessStatusCode)
            {                
                responseString = response.Content.ReadAsStringAsync().Result.ToString();
            }
            else
            {
                Logger.StatusEndFailed("Failed : Some issue occured while trying to fetch projects.", fileLogger);
            }

            // Convert responseString into a json Object
            ProjectCollection jsonObj = JsonConvert.DeserializeObject<ProjectCollection>(responseString);
            //Console.WriteLine("Found " + jsonObj.Count + " projects");
            Logger.Message("Found " + jsonObj.Count + " projects", fileLogger);


            var engine = new FileHelperEngine<Project>();

            var projects = new List<Project>();

            //Do stuff
            foreach (var obj in jsonObj.Value)
            {
                projects.Add(obj);
            }
            engine.WriteFile("Projects.csv", projects);
            Logger.StatusEndSuccess("Success : Project details extracted.", fileLogger);
        }


        static public void GetAllUsers(HttpClient client, ConsoleLogger Logger, ILog fileLogger)
        {
            var graphClient = new HttpClient();
            string personalaccesstoken = ConfigurationManager.AppSettings["personalAccessToken"];

            var credentials = Convert.ToBase64String(
                   System.Text.ASCIIEncoding.ASCII.GetBytes(
                       string.Format("{0}:{1}", "", personalaccesstoken)));
            string azure_devops_vssps_Url = "https://vssps.dev.azure.com/" + ConfigurationManager.AppSettings["orgName"] + "/";
            graphClient.BaseAddress = new Uri(azure_devops_vssps_Url);
            graphClient.DefaultRequestHeaders.Accept.Clear();
            graphClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            graphClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            string responseString = null;
            HttpResponseMessage response = graphClient.GetAsync("_apis/graph/users?api-version=5.0-preview.1&subjectTypes=aad&continuationToken=").Result;


            if (response.IsSuccessStatusCode)
            {
                responseString = response.Content.ReadAsStringAsync().Result.ToString();
            }
            else
            {
                Logger.StatusEndFailed("Failed : Some issue occured while trying to fetch users.", fileLogger);
            }

            // Convert responseString into a json Object
            UserCollection jsonObj = JsonConvert.DeserializeObject<UserCollection>(responseString);
            //Console.WriteLine("Found " + jsonObj.Count + " projects");
            Logger.Message("Found " + jsonObj.Count + " users.", fileLogger);

            var engine = new FileHelperEngine<User>();

            var users = new List<User>();

            //Do stuff
            foreach (var obj in jsonObj.Value)
            {
                users.Add(obj);
            }
            engine.WriteFile("Users.csv", users);
            Logger.StatusEndSuccess("Success : Project details extracted.", fileLogger);
        }

        static public void GetAllUsersFromProjectGroups(ConsoleLogger Logger, ILog fileLogger)
        {
            var graphClient = new HttpClient();
            string personalaccesstoken = ConfigurationManager.AppSettings["personalAccessToken"];

            var credentials = Convert.ToBase64String(
                   System.Text.ASCIIEncoding.ASCII.GetBytes(
                       string.Format("{0}:{1}", "", personalaccesstoken)));
            string azure_devops_vssps_Url = "https://vssps.dev.azure.com/" + ConfigurationManager.AppSettings["orgName"] + "/";
            graphClient.BaseAddress = new Uri(azure_devops_vssps_Url);
            graphClient.DefaultRequestHeaders.Accept.Clear();
            graphClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            graphClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            string responseString = null;

            string endpointUri = "_apis/graph/groups?scopeDescriptor=" + ConfigurationManager.AppSettings["projectDescriptor"] + "&api-version=5.0-preview.1";
            //Get all groups for a project
            HttpResponseMessage response = graphClient.GetAsync(endpointUri).Result;

            if (response.IsSuccessStatusCode)
            {
                responseString = response.Content.ReadAsStringAsync().Result.ToString();
            }
            else
            {
                Logger.StatusEndFailed("Failed : Some issue occured while trying to fetch groups of project.", fileLogger);
            }

            // Convert responseString into a json Object
            GroupCollection jsonObj = JsonConvert.DeserializeObject<GroupCollection>(responseString);            
            Logger.Message("Found " + jsonObj.Count + " Group in this project", fileLogger);

            var lstMemberFormattedColl = new List<FormattedMembers>();

            foreach (var obj in jsonObj.Value)
            {
               
                    var vsaexClient = new HttpClient();
                    string azure_devops_vsaex_Url = "https://vsaex.dev.azure.com/" + ConfigurationManager.AppSettings["orgName"] + "/";
                    vsaexClient.BaseAddress = new Uri(azure_devops_vsaex_Url);
                    vsaexClient.DefaultRequestHeaders.Accept.Clear();
                    vsaexClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    vsaexClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                    responseString = null;

                    //Get all users for a group
                    response = vsaexClient.GetAsync($"_apis/GroupEntitlements/{obj.OriginId}/members?api-version=6.0-preview.1").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        responseString = response.Content.ReadAsStringAsync().Result.ToString();
                    }
                    else
                    {
                        Logger.StatusEndFailed("Failed : Some issue occured while trying to fetch users of project groups.", fileLogger);
                    }

                    // Convert responseString into a json Object
                    MembersCollection lstGroupMembers = JsonConvert.DeserializeObject<MembersCollection>(responseString);
                    Logger.Message("Found " + lstGroupMembers.Members.Length + " Users in this Group : " + obj.DisplayName, fileLogger);

                    if (lstGroupMembers != null && lstGroupMembers.Members != null)
                    {
                        foreach (var objMember in lstGroupMembers.Members)
                        {

                            FormattedMembers memberFormated = new FormattedMembers();
                            memberFormated.GroupId = obj.OriginId;
                            memberFormated.GroupName = obj.DisplayName;
                            memberFormated.UserName = objMember.User.MailAddress;
                            lstMemberFormattedColl.Add(memberFormated);
                        }
                    }
               
            }

            var engine = new FileHelperEngine<FormattedMembers>();
            engine.WriteFile("GroupUsers.csv", lstMemberFormattedColl);
            Logger.StatusEndSuccess("Success : Project details extracted.", fileLogger);
        }       

        static public void GetRepos(HttpClient client, ConsoleLogger Logger, ILog fileLogger)
        {
            string responseString = null;
            HttpResponseMessage response = client.GetAsync("_apis/projects?$top=250&stateFilter=All&api-version=1.0").Result;


            if (response.IsSuccessStatusCode)
            {
                responseString = response.Content.ReadAsStringAsync().Result.ToString();
            }
            else
            {
                Logger.StatusEndFailed("Failed : Some issue occured while trying to fetch projects.", fileLogger);
            }

            // Convert responseString into a json Object
            ProjectCollection jsonObj = JsonConvert.DeserializeObject<ProjectCollection>(responseString);
            //Console.WriteLine("Found " + jsonObj.Count + " projects"); 
            Logger.Message("Found " + jsonObj.Count + " projects", fileLogger);

            var reposCollection = new List<RepositoryCollection>();
            var lstRepoformattedColl = new List<RepositoryFormatted>();
            //Do stuff
            foreach (var obj in jsonObj.Value)            {

               

                    HttpResponseMessage repoResponse = client.GetAsync($"{obj.Name}/_apis/git/repositories?api-version=1.0").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        responseString = repoResponse.Content.ReadAsStringAsync().Result.ToString();
                    }
                    else
                    {
                        Logger.StatusEndFailed("Failed : Some issue occured while trying to repositories of project.", fileLogger);
                    }
                    // Convert responseString into a json Object
                    RepositoryCollection repos = JsonConvert.DeserializeObject<RepositoryCollection>(responseString);
                    //Console.WriteLine("Found " + repoColl.Count + " Repos");
                    Logger.Message("Found " + repos.Count + " repos in Project : " + obj.Name, fileLogger);

                    foreach (var objRepo in repos.Value)
                    {
                        RepositoryFormatted repoformated = new RepositoryFormatted();
                        repoformated.RepoId = objRepo.Id;
                        repoformated.RepoName = objRepo.Name;
                        repoformated.RepoSize = objRepo.Size;
                        repoformated.RepoWebUrl = objRepo.WebUrl;
                        repoformated.RepoUrl = objRepo.RepoUrl;
                        repoformated.ProjectId = objRepo.Project.Id;
                        repoformated.ProjectName = objRepo.Project.Name;
                        repoformated.ProjectDescription = objRepo.Project.Description;
                        repoformated.ProjectUrl = objRepo.Project.Url;
                        repoformated.ProjectVisibility = objRepo.Project.Visibility;
                        repoformated.ProjectLastUpdateTime = objRepo.Project.LastUpdateTime;

                        lstRepoformattedColl.Add(repoformated);
                    }
                


            }

            var engine = new FileHelperEngine<RepositoryFormatted>();            
            engine.WriteFile("Repo.csv", lstRepoformattedColl);
            Logger.StatusEndSuccess("Success : Repo details extracted.", fileLogger);
        }

        static public void GetBuildPipelines(HttpClient client, ConsoleLogger Logger, ILog fileLogger)
        {
            string responseString = null;
            HttpResponseMessage response = client.GetAsync("_apis/projects?$top=250&stateFilter=All&api-version=1.0").Result;


            if (response.IsSuccessStatusCode)
            {
                responseString = response.Content.ReadAsStringAsync().Result.ToString();
            }
            else
            {
                Logger.StatusEndFailed("Failed : Some issue occured while trying to fetch projects.", fileLogger);
            }

            // Convert responseString into a json Object
            ProjectCollection jsonObj = JsonConvert.DeserializeObject<ProjectCollection>(responseString);
            //Console.WriteLine("Found " + jsonObj.Count + " projects");
            Logger.Message("Found " + jsonObj.Count + " projects", fileLogger);

            var lstBPformattedcoll = new List<BuildPipelineFormatted>();
            //Do stuff
            foreach (var obj in jsonObj.Value)
            {

                

                    HttpResponseMessage repoResponse = client.GetAsync($"{obj.Name}/_apis/build/definitions?api-version=5.0").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        responseString = repoResponse.Content.ReadAsStringAsync().Result.ToString();
                    }
                    else
                    {
                        Logger.StatusEndFailed("Failed : Some issue occured while trying to fetch buid pipelines.", fileLogger);
                    }
                    // Convert responseString into a json Object
                    BuildPipelineCollection buildPipelines = JsonConvert.DeserializeObject<BuildPipelineCollection>(responseString);
                    //Console.WriteLine("Found " + buildPipelines.Count + " Build Pipelines");
                    Logger.Message("Found " + buildPipelines.Count + " Build Pipelines", fileLogger);

                    foreach (var objbp in buildPipelines.Value)
                    {
                        BuildPipelineFormatted bpformated = new BuildPipelineFormatted();
                        bpformated.BuildPipelneId = objbp.Id;
                        bpformated.BuildPipelineName = objbp.Name;
                        bpformated.Type = objbp.Type;
                        bpformated.QueueStatus = objbp.QueueStatus;
                        bpformated.CreatedDate = objbp.CreatedDate;
                        bpformated.DisplayName = objbp.AuthoredBy.DisplayName;
                        bpformated.UniqueName = objbp.AuthoredBy.UniqueName;
                        bpformated.ProjectId = objbp.Project.Id;
                        bpformated.ProjectName = objbp.Project.Name;
                        bpformated.ProjectDescription = objbp.Project.Description;
                        bpformated.ProjectUrl = objbp.Project.Url;
                        bpformated.ProjectVisibility = objbp.Project.Visibility;
                        bpformated.ProjectLastUpdateTime = objbp.Project.LastUpdateTime;

                        lstBPformattedcoll.Add(bpformated);
                    }
                


            }

            var engine = new FileHelperEngine<BuildPipelineFormatted>();
            engine.WriteFile("BP.csv", lstBPformattedcoll);
            Logger.StatusEndSuccess("Success : Build Pipelines details extracted.", fileLogger);
        }

        static public void GetReleasePipelines(HttpClient client, ConsoleLogger Logger, ILog fileLogger)
        {
            string responseString = null;
            HttpResponseMessage response = client.GetAsync("_apis/projects?$top=250&stateFilter=All&api-version=1.0").Result;


            if (response.IsSuccessStatusCode)
            {
                responseString = response.Content.ReadAsStringAsync().Result.ToString();
            }
            else
            {
                Logger.StatusEndFailed("Failed : Some issue occured while trying to fetch projects.", fileLogger);
            }

            // Convert responseString into a json Object
            ProjectCollection jsonObj = JsonConvert.DeserializeObject<ProjectCollection>(responseString);
            //Console.WriteLine("Found " + jsonObj.Count + " projects");
            Logger.Message("Found " + jsonObj.Count + " projects", fileLogger);

            var lstRPformattedcoll = new List<ReleasePipelineFormatted>();
            //Do stuff
            foreach (var obj in jsonObj.Value)
            {
               
                var releaseClient = new HttpClient();
                string personalaccesstoken = ConfigurationManager.AppSettings["personalAccessToken"];

                var credentials = Convert.ToBase64String(
                        System.Text.ASCIIEncoding.ASCII.GetBytes(
                            string.Format("{0}:{1}", "", personalaccesstoken)));

                string azure_devops_vsrm_Url = "https://vsrm.dev.azure.com/" + ConfigurationManager.AppSettings["orgName"] + "/";
                releaseClient.BaseAddress = new Uri(azure_devops_vsrm_Url);
                releaseClient.DefaultRequestHeaders.Accept.Clear();
                releaseClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                releaseClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                HttpResponseMessage repoResponse = releaseClient.GetAsync($"{obj.Name}/_apis/release/definitions?api-version=5.0").Result;

                if (response.IsSuccessStatusCode)
                {
                    responseString = repoResponse.Content.ReadAsStringAsync().Result.ToString();
                }
                else
                {
                    Logger.StatusEndFailed("Failed : Some issue occured while trying to fetch release pipelines.", fileLogger);
                }
                // Convert responseString into a json Object
                ReleasePipelineCollection releasePipelines = JsonConvert.DeserializeObject<ReleasePipelineCollection>(responseString);
                //Console.WriteLine("Found " + releasePipelines.Count + " Release Pipelines");
                Logger.Message("Found " + releasePipelines.Count + " Release Pipelines in Project : " + obj.Name, fileLogger);

                foreach (var objrp in releasePipelines.Value)
                {
                    ReleasePipelineFormatted rpformated = new ReleasePipelineFormatted();
                    rpformated.ReleasePipelneId = objrp.Id;
                    rpformated.ReleasePipelineName = objrp.Name;
                    rpformated.CreatedDate = objrp.CreatedDate;
                    rpformated.DisplayName = objrp.CreatedBy != null ? objrp.CreatedBy.DisplayName : null;
                    rpformated.UniqueName = objrp.CreatedBy != null ? objrp.CreatedBy.UniqueName : null;
                    rpformated.CreatedDate = objrp.ModifiedDate;
                    rpformated.DisplayName = objrp.ModifiedBy != null ? objrp.ModifiedBy.DisplayName : null;
                    rpformated.UniqueName = objrp.ModifiedBy != null ? objrp.ModifiedBy.UniqueName : null;
                    rpformated.ProjectId = obj.Id;
                    rpformated.ProjectName = obj.Name;
                    rpformated.ProjectDescription = obj.Description;
                    rpformated.ProjectUrl = obj.Url;
                    rpformated.ProjectVisibility = obj.Visibility;
                    rpformated.ProjectLastUpdateTime = obj.LastUpdateTime;

                    lstRPformattedcoll.Add(rpformated);
                }
            }
            var engine = new FileHelperEngine<ReleasePipelineFormatted>();
            engine.WriteFile("RP.csv", lstRPformattedcoll);
            Logger.StatusEndSuccess("Success : Release Pipelines details extracted.", fileLogger);
        }

        static public void GetAllVariableGroups(HttpClient client, ConsoleLogger Logger, ILog fileLogger)
        {
            string responseString = null;
            HttpResponseMessage response = client.GetAsync("_apis/projects?$top=250&stateFilter=All&api-version=1.0").Result;


            if (response.IsSuccessStatusCode)
            {
                responseString = response.Content.ReadAsStringAsync().Result.ToString();
            }
            else
            {
                Logger.StatusEndFailed("Failed : Some issue occured while trying to fetch projects.", fileLogger);
            }

            // Convert responseString into a json Object
            ProjectCollection jsonObj = JsonConvert.DeserializeObject<ProjectCollection>(responseString);
            //Console.WriteLine("Found " + jsonObj.Count + " projects"); 
            Logger.Message("Found " + jsonObj.Count + " projects", fileLogger);
            
            var lstVGformattedColl = new List<VariableGroupFormatted>();
            //Do stuff
            foreach (var obj in jsonObj.Value)
            {


                HttpResponseMessage vgResponse = client.GetAsync($"{obj.Name}/_apis/distributedtask/variablegroups?api-version=5.1-preview.1").Result;
                
                
                if (response.IsSuccessStatusCode)
                {
                    responseString = vgResponse.Content.ReadAsStringAsync().Result.ToString();
                }
                else
                {
                    Logger.StatusEndFailed("Failed : Some issue occured while trying to variable groups of project.", fileLogger);
                }
                // Convert responseString into a json Object
                VariableGroupCollection variableGroups = JsonConvert.DeserializeObject<VariableGroupCollection>(responseString);
                
                Logger.Message("Found " + variableGroups.Count + " Variable groups in Project : " + obj.Name, fileLogger);

                foreach (var objVG in variableGroups.Value)
                {
                    VariableGroupFormatted vgFormated = new VariableGroupFormatted();
                    vgFormated.VGId = objVG.Id;
                    vgFormated.VGName = objVG.Name;
                    vgFormated.CreatedBy = objVG.CreatedBy.UniqueName;
                    vgFormated.ProjectId = obj.Id;
                    vgFormated.ProjectName = obj.Name;                    
                    lstVGformattedColl.Add(vgFormated);
                }
            }
            var engine = new FileHelperEngine<VariableGroupFormatted>();
            engine.WriteFile("VariableGroup.csv", lstVGformattedColl);
            Logger.StatusEndSuccess("Success : VariableGroup details extracted.", fileLogger);
        }

        static public void MigrateProjectVariableGroups(HttpClient client, ConsoleLogger Logger, ILog fileLogger)
        {
            string responseString = null;
            string srcProjectName = "Source-Project";
            string dstProjectName = "Destination-Project";
            string orgName = ConfigurationManager.AppSettings["orgName"];
            string personelAccessToken = ConfigurationManager.AppSettings["personalAccessToken"];

            //HttpResponseMessage vgResponse = client.GetAsync($"{srcProjectName}/_apis/distributedtask/variablegroups?api-version=5.1-preview.1").Result;
            HttpResponseMessage vgResponse = client.GetAsync($"{srcProjectName}/_apis/distributedtask/variablegroups?groupName=p*&queryOrder=IdDescending&api-version=6.0-preview.2").Result;
            if (vgResponse.IsSuccessStatusCode)
                {
                    responseString = vgResponse.Content.ReadAsStringAsync().Result.ToString();
                }
                else
                {
                    Logger.StatusEndFailed("Failed : Some issue occured while trying to variable groups of project.", fileLogger);
                }
                // Convert responseString into a json Object
                VariableGroupCollection variableGroups = JsonConvert.DeserializeObject<VariableGroupCollection>(responseString);

                Logger.Message("Found " + variableGroups.Count + " Variable groups in Project : " + srcProjectName, fileLogger);

                foreach (var objVG in variableGroups.Value)
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\soc2-utility\soc2-utility\bin\Debug\netcoreapp3.1\vg-migrate\AssureCare.VstsTools.VariableGroupCopier.exe");        // exe file copied from AssureCare.VstsTools.VariableGroupCopier

                    //here you add your arguments
                    startInfo.ArgumentList.Add(srcProjectName);       // First argument          
                    startInfo.ArgumentList.Add(objVG.Name);       // second argument
                    startInfo.ArgumentList.Add(dstProjectName);       // third argument
                    startInfo.ArgumentList.Add(objVG.Name);     // forth argument
                    startInfo.ArgumentList.Add(personelAccessToken);     // Fifth argument PAT token
                    startInfo.ArgumentList.Add("N");     // Sixth Override existing argument
                    startInfo.ArgumentList.Add(orgName);     // forth argument - Orgnization name
                    Process.Start(startInfo);
                    Console.WriteLine("Sleeping for 5 sec - " + objVG.Name);
                    System.Threading.Thread.Sleep(5000);
            }
            
        }


        static public void GetAllTaskGroups(HttpClient client, ConsoleLogger Logger, ILog fileLogger)
        {
            string responseString = null;
            HttpResponseMessage response = client.GetAsync("_apis/projects?$top=250&stateFilter=All&api-version=1.0").Result;


            if (response.IsSuccessStatusCode)
            {
                responseString = response.Content.ReadAsStringAsync().Result.ToString();
            }
            else
            {
                Logger.StatusEndFailed("Failed : Some issue occured while trying to fetch projects.", fileLogger);
            }

            // Convert responseString into a json Object
            ProjectCollection jsonObj = JsonConvert.DeserializeObject<ProjectCollection>(responseString);
            //Console.WriteLine("Found " + jsonObj.Count + " projects"); 
            Logger.Message("Found " + jsonObj.Count + " projects", fileLogger);

            var lstTGformattedColl = new List<TaskGroupFormatted>();
            //Do stuff
            foreach (var obj in jsonObj.Value)
            {


                HttpResponseMessage tgResponse = client.GetAsync($"{obj.Name}/_apis/distributedtask/taskgroups?api-version=5.1-preview.1").Result;

                if (response.IsSuccessStatusCode)
                {
                    responseString = tgResponse.Content.ReadAsStringAsync().Result.ToString();
                }
                else
                {
                    Logger.StatusEndFailed("Failed : Some issue occured while trying to task groups of project.", fileLogger);
                }
                // Convert responseString into a json Object
                TaskGroupCollection taskGroups = JsonConvert.DeserializeObject<TaskGroupCollection>(responseString);

                Logger.Message("Found " + taskGroups.Count + " task groups in Project : " + obj.Name, fileLogger);

                foreach (var objTG in taskGroups.Value)
                {
                    TaskGroupFormatted tgFormated = new TaskGroupFormatted();
                    tgFormated.VGId = objTG.Id;
                    tgFormated.VGName = objTG.Name;
                    tgFormated.ProjectId = obj.Id;
                    tgFormated.ProjectName = obj.Name;
                    lstTGformattedColl.Add(tgFormated);
                }
            }
            var engine = new FileHelperEngine<TaskGroupFormatted>();
            engine.WriteFile("TaskGroups.csv", lstTGformattedColl);
            Logger.StatusEndSuccess("Success : TaskGroups details extracted.", fileLogger);
        }

        static public void RemoveAllUsersFromProjectGroups(ConsoleLogger Logger, ILog fileLogger)
        {
            var graphClient = new HttpClient();
            string personalaccesstoken = ConfigurationManager.AppSettings["personalAccessToken"];

            var credentials = Convert.ToBase64String(
                   System.Text.ASCIIEncoding.ASCII.GetBytes(
                       string.Format("{0}:{1}", "", personalaccesstoken)));
            string azure_devops_vssps_Url = "https://vssps.dev.azure.com/" + ConfigurationManager.AppSettings["orgName"] + "/";
            graphClient.BaseAddress = new Uri(azure_devops_vssps_Url);
            graphClient.DefaultRequestHeaders.Accept.Clear();
            graphClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            graphClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            string responseString = null;

            string endpointUri = "_apis/graph/groups?scopeDescriptor=" + ConfigurationManager.AppSettings["projectDescriptor"] + "&api-version=5.0-preview.1";
            //Get all groups for a project
            HttpResponseMessage response = graphClient.GetAsync(endpointUri).Result;

            if (response.IsSuccessStatusCode)
            {
                responseString = response.Content.ReadAsStringAsync().Result.ToString();
            }
            else
            {
                Logger.StatusEndFailed("Failed : Some issue occured while trying to fetch groups of the project.", fileLogger);
            }

            // Convert responseString into a json Object
            GroupCollection jsonObj = JsonConvert.DeserializeObject<GroupCollection>(responseString);
            //Console.WriteLine("Found " + jsonObj.Count + " Group in this project");
            Logger.Message("Found " + jsonObj.Count + " Group in this project", fileLogger);

            foreach (var obj in jsonObj.Value)
            {

                string strExclusionRoles = ConfigurationManager.AppSettings["exclusionRoles"];
                string[] exclusionRolesArray = strExclusionRoles.Split(new string[] { "," },
                                                  StringSplitOptions.None);

                if (!Array.Exists(exclusionRolesArray, element => element == obj.DisplayName))
                {

                    var vsaexClient = new HttpClient();
                    string azure_devops_vsaex_Url = "https://vsaex.dev.azure.com/" + ConfigurationManager.AppSettings["orgName"] + "/";
                    vsaexClient.BaseAddress = new Uri(azure_devops_vsaex_Url);
                    vsaexClient.DefaultRequestHeaders.Accept.Clear();
                    vsaexClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    vsaexClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                    responseString = null;

                    //Get all users for a group
                    response = vsaexClient.GetAsync($"_apis/GroupEntitlements/{obj.OriginId}/members?api-version=6.0-preview.1").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        responseString = response.Content.ReadAsStringAsync().Result.ToString();
                    }
                    else
                    {
                        Logger.StatusEndFailed("Failed : Some issue occured while trying to fetch users of group " + obj.DisplayName, fileLogger);
                    }

                    // Convert responseString into a json Object
                    MembersCollection lstGroupMembers = JsonConvert.DeserializeObject<MembersCollection>(responseString);
                    // Console.WriteLine("Found " + lstGroupMembers.Members.Length + " Users in this Group : " + obj.DisplayName);

                    Logger.Message("Found " + lstGroupMembers.Members.Length + " Users in this Group : " + obj.DisplayName, fileLogger);

                    if (lstGroupMembers != null && lstGroupMembers.Members != null)
                    {
                        foreach (var objMember in lstGroupMembers.Members)
                        {
                            responseString = null;

                            //Delete all users for a group
                            response = vsaexClient.DeleteAsync($"_apis/GroupEntitlements/{obj.OriginId}/members/{objMember.Id}?api-version=6.0-preview.1").Result;

                            if (response.IsSuccessStatusCode)
                            {
                                //Console.WriteLine("Success : User " + objMember.User.DisplayName + " is removed from " + obj.DisplayName);
                                Logger.StatusEndSuccess("Success : User " + objMember.User.DisplayName + " is removed from " + obj.DisplayName, fileLogger);

                            }
                            else
                            {
                                //Console.WriteLine("Failure : User " + objMember.User.DisplayName + " is not removed from " + obj.DisplayName);
                                Logger.StatusEndFailed("Failure : User " + objMember.User.DisplayName + " is not removed from " + obj.DisplayName, fileLogger);
                            }
                        }
                    }
                }
            }
        }
    }
}
