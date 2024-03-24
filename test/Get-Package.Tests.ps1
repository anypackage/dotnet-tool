#Requires -Modules AnyPackage.DotNet.Tool

Describe Get-Package {
    BeforeAll {
        dotnet tool install powerprepare.app --global
        dotnet tool install ib --global
    }

    AfterAll {
        dotnet tool uninstall powerprepare.app --global
        dotnet tool uninstall ib --global
    }

    Context 'with no parameters' {
        It 'should return results' {
            Get-Package |
            Should -Not -BeNullOrEmpty
        }
    }

    Context 'with -Name parameter' {
        It 'should return powerprepare.app' {
            Get-Package -Name powerprepare.app |
            Should -Not -BeNullOrEmpty
        }
    }
}
