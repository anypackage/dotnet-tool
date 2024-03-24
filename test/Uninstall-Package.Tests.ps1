#Requires -Modules AnyPackage.DotNet.Tool

Describe Uninstall-Package {
    BeforeEach {
        dotnet tool install powerprepare.app --global
    }

    Context 'with -Name parameter' {
        It 'should uninstall' {
            { Uninstall-Package -Name powerprepare.app } |
            Should -Not -Throw
        }
    }
}
