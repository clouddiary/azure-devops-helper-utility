

![Azure_DevOps_Utility.png](Azure_DevOps_Utility.png)

# azure-devops-utility
Azure DevOps Utility is a dotnet console app which helps in doing listed operation on Azure DevOps Orgnization. 
It was very useful tool while rolling out Enterprise security controls across Azure DevOps Orgnization. 

## Supported Operations

* GetProjects - Get all projects (with all details) in an Azure DevOps Orgnizaton
* GetRepos- Get all repos (with all details) in an Azure DevOps Orgnizaton
* GetBuildPipelines - Get all build pipelines (with all details) in an Azure DevOps Orgnizaton
* GetReleasePipelines - Get all release pipelines (with all details) in an Azure DevOps Orgnizaton
* GetAllVariableGroups - Get all variable groups (with all details) in an Azure DevOps Orgnizaton
* GetAllTaskGroups - Get all task groups (with all details) in an Azure DevOps Orgnizaton
* GetAllUsers - Get all users (with all details) in an Azure DevOps Orgnizaton
* GetAllUsersFromProjectGroups - Get all users from a specific(with all details) in an Azure DevOps Porject
* RemoveAllUsersFromProjectGroups - Remove all users from a specific in an Azure DevOps Porject
* MigrateProjectVariableGroups - Move variable group from one Azure DevOps porject to another

## App Config input configs

* orgName : [tezs] Azure DevOps Orgnization name
* personalAccessToken : [XXXXXXXXXXX] Azure DevOps Personnel access token for a user who have Project collection admin access
* operation : [GetProjects] - Any one operation name from above list of supported operations.
* projectDescriptor : [scp.XXXXXXXXX] - Azure DevOps Project descriptor Id (Azure DevOps identify each group from its descriptor id which can be found using postman API call - refer : https://learn.microsoft.com/en-us/rest/api/azure/devops/graph/descriptors/get?view=azure-devops-rest-6.0&tabs=HTTP )
* exclusionRoles : [Lead Developer,Lead DBA,Technical Writers] - Comma separated role name which needs to excluded from RemoveAllUsersFromProjectGroups operation

## Download solution & Open in Visual Studio to run it
