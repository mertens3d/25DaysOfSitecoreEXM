﻿#$websiteRoot = "C:\inetpub\wwwroot\LearnEXMxconnect.dev.local"

#$scriptPath = (Get-Variable MyInvocation -Scope Script).Value.MyCommand.Path
#$currentFolder = Split-Path $scriptPath
#Push-Location $currentFolder

#$src = "..\..\path"


Stop-Service -Name $serviceAutomationEngine
Stop-Service -Name $serviceProcessingEngine
Stop-Service -Name $serviceIndexer
iisreset /stop

$xConnectWebRoot = "C:\inetpub\wwwroot\LearnEXMxconnect.dev.local"

$solutionSourceRoot = "C:\projects\25DaysOfSitecoreEXM\src"

$jsonModelsAr = @("$solutionSourceRoot\Feature\JSONModelGenerator\code\AutoGenerated\*.json") # add additional filters to the array as needed

$ModeDllsAr = @("$solutionSourceRoot\Foundation\Marketing\code\bin\debug\LearnEXM.Foundation.Marketing.dll",
                "$solutionSourceRoot\Feature\Marketing\code\bin\debug\LearnEXM.Feature.Marketing.dll",
                "$solutionSourceRoot\Foundation\CollectionModel.Builder\code\bin\Debug\LearnEXM.Foundation.CollectionModel.Builder.dll"

                )

$xmlPredicateDescriptors = @("$solutionSourceRoot\Feature\Marketing\code\PredicateDescriptors\*.xml")

$xmlConfigurationFiles = @("$solutionSourceRoot\Feature\JSONModelGenerator\Code\{xConnect}\App_Data\jobs\continuous\AutomationEngine\App_Data\config\sitecore\sc.*.xml")

$serviceAutomationEngine = "LearnEXMxconnect.dev.local-MarketingAutomationService"
$serviceProcessingEngine = "LearnEXMxconnect.dev.local-ProcessingEngineService"
$serviceIndexer = "LearnEXMxconnect.dev.local-IndexWorker"

# -------------- Targets - you shouldn't need to change these

$xConnectAppDataModels = "$xConnectWebRoot\App_Data\Models"
$xConnectAppDataIndexeWorkerModels = "$xConnectWebRoot\App_data\jobs\continuous\IndexWorker\App_data\Models"
$xConnectApp_dataAutomationEngine = "$xConnectWebRoot\App_data\jobs\continuous\AutomationEngine"
$xConnectApp_dataAutomationEngineAppDataSitecore = "$xConnectWebRoot\App_data\jobs\continuous\AutomationEngine\app_data\config\sitecore"
$xConnectBin  = "$xConnectWebRoot\bin"

$xConnectApp_dataSegmentation = "$xConnectWebRoot\App_Data\Config\sitecore\Segmentation"
$xConnectAutomationEngineSegmentation = "$xConnectWebRoot\App_data\jobs\continuous\AutomationEngine\App_Data\Config\sitecore\Segmentation"



function CopyFiles ($SourceArray, $destinationFolder) {
    Write-Host("")
    Write-Host("CopyFiles:")
    # Write-Host("SourceArray : $SourceArray")
    Write-Host("`tdestinationFolder : $destinationFolder")

     foreach ($source in $SourceArray){
        Write-Host("`t`tsource : $source")
        
        $filteredChidlren = Get-ChildItem -Path $source -Recurse



        if($filteredChidlren -ne $null){
            foreach ($filteredChild in $filteredChidlren){
                Write-Host "`t`t`tfilteredChild : $filteredChild.Name" 
                Copy-Item $filteredChild -Destination $destinationFolder
            }

        }
        else{
            write-host ("no filtered children")
        }
     }
}




# --------------- Tasks



Stop-Service -Name $serviceAutomationEngine
Stop-Service -Name $serviceProcessingEngine
Stop-Service -Name $serviceIndexer
iisreset /stop


CopyFiles $jsonModelsAr $xConnectAppDataModels
CopyFiles $jsonModelsAr $xConnectAppDataIndexeWorkerModels
CopyFiles $ModeDllsAr $xConnectApp_dataAutomationEngine
CopyFiles $ModeDllsAr $xConnectApp_dataAutomationEngine

CopyFiles $xmlPredicateDescriptors $xConnectApp_dataSegmentation
CopyFiles $xmlPredicateDescriptors $xConnectAutomationEngineSegmentation


CopyFiles $xmlConfigurationFiles $xConnectApp_dataAutomationEngineAppDataSitecore


iisreset /restart

Restart-Service -Name $serviceAutomationEngine
Restart-Service -Name $serviceProcessingEngine
Restart-Service -Name $serviceIndexer