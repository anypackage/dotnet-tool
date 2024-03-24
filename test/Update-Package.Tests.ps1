#Requires -Modules AnyPackage.DotNet.Tool

Describe Update-Package {
    BeforeEach {
        dotnet tool install powerprepare.app --global --version 1.0.1
    }

    AfterEach {
        dotnet tool uninstall powerprepare.app --global
    }

    Context 'without parameters' {
        It 'should update' -Skip {
            { Update-Package -ErrorAction Stop } |
            Should -Not -Throw
        }
    }

    Context 'with -Name parameter' {
        It 'should update' {
            { Update-Package -Name powerprepare.app -ErrorAction Stop } |
            Should -Not -Throw
        }
    }
}
