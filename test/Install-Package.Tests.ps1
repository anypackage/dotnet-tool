#Requires -Modules AnyPackage.DotNet.Tool

Describe Install-Package {
    AfterEach {
        dotnet tool uninstall powerprepare.app --global
    }

    Context 'with -Name parameter' {
        It 'should install' {
            { Install-Package -Name powerprepare.app } |
            Should -Not -Throw
        }
    }

    Context 'with -Version parameter' {
        It 'should install' {
            { Install-Package -Name powerprepare.app -Version '1.0.1' -ErrorAction Stop } |
            Should -Not -Throw
        }
    }
}
