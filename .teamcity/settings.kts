import jetbrains.buildServer.configs.kotlin.v2019_2.*
import jetbrains.buildServer.configs.kotlin.v2019_2.buildSteps.nuGetPublish
import jetbrains.buildServer.configs.kotlin.v2019_2.buildSteps.powerShell
import jetbrains.buildServer.configs.kotlin.v2019_2.triggers.vcs
import jetbrains.buildServer.configs.kotlin.v2019_2.buildFeatures.pullRequests

project {
    buildType(ChocolateyNugetClient)
}

object ChocolateyNugetClient : BuildType({
    name = "Build"

    artifactRules = """
        artifacts/nupkgs/Chocolatey.NuGet.Commands.*.nupkg
        artifacts/nupkgs/Chocolatey.NuGet.Common.*.nupkg
        artifacts/nupkgs/Chocolatey.NuGet.Configuration.*.nupkg
        artifacts/nupkgs/Chocolatey.NuGet.Credentials.*.nupkg
        artifacts/nupkgs/Chocolatey.NuGet.DependencyResolver.Core.*.nupkg
        artifacts/nupkgs/Chocolatey.NuGet.LibraryModel.*.nupkg
        artifacts/nupkgs/Chocolatey.NuGet.PackageManagement.*.nupkg
        artifacts/nupkgs/Chocolatey.NuGet.Packaging.*.nupkg
        artifacts/nupkgs/Chocolatey.NuGet.ProjectModel.*.nupkg
        artifacts/nupkgs/Chocolatey.NuGet.Protocol.*.nupkg
        artifacts/nupkgs/Chocolatey.NuGet.Resolver.*.nupkg
        artifacts/nupkgs/Chocolatey.NuGet.Versioning.*.nupkg
    """.trimIndent()

    vcs {
        root(DslContext.settingsRoot)

        branchFilter = """
            +:*
        """.trimIndent()
    }

    steps {
        powerShell {
            name = "Prerequisites"
            scriptMode = script {
                content = """
                    # Install Chocolatey Requirements
                    if ((Get-WindowsFeature -Name NET-Framework-Features).InstallState -ne 'Installed') {
                        Install-WindowsFeature -Name NET-Framework-Features
                    }
                    
                    choco install visualstudio2022-workload-manageddesktopbuildtools visualstudio2022-workload-visualstudioextensionbuildtools visualstudio2022-component-texttemplating-buildtools --confirm --no-progress
                    
                    exit ${'$'}LastExitCode
                """.trimIndent()
            }
        }
        powerShell {
            name = "Configure .NET and other dependencies"
            scriptMode = file {
                path = "configure.ps1"
            }
        }
        powerShell {
            name = "Build"
            scriptMode = file {
                path = "build.ps1"
            }
            scriptArgs = "-CI -SkipUnitTest"
        }
        powerShell {
            scriptMode = script {
                content = """
                    ${'$'}files=Get-ChildItem "artifacts/nupkgs" | Where-Object {${'$'}_.Name -like "*.nupkg" -and ${'$'}_.Name -notlike "*symbols*"}

                    foreach (${'$'}file in ${'$'}files) {
                      & "%teamcity.tool.NuGet.CommandLine.DEFAULT%" push -Source %env.NUGETDEVPUSH_SOURCE% -ApiKey %env.NUGETDEVPUSH_API_KEY% "${'$'}(${'$'}file.FullName)"
                    }
                """.trimIndent()
            }
        }
    }

    triggers {
        vcs {
            branchFilter = ""
        }
    }

    features {
        pullRequests {
            provider = github {
                authType = token {
                    token = "%system.GitHubPAT%"
                }
            }
        }
    }
})
