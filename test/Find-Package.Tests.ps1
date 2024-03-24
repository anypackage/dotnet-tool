#Requires -Modules AnyPackage.DotNet.Tool

Describe Find-Package {
    Context 'with -Prerelease parameter' {
        It 'should return prerelease versions' {
            Find-Package -Name PowerShell -Prerelease -WarningAction SilentlyContinue |
            Select-Object -ExpandProperty Version |
            Where-Object IsPrerelease -eq $true |
            Should -Not -BeNullOrEmpty
        }
    }

    Context 'with -Name parameter' {
        It 'single name' {
            Find-Package -Name powershell |
            Should -Not -BeNullOrEmpty
        }

        It 'multiple names' {
            Find-Package -Name powershell, microsoft.dotnet-interactive |
            Select-Object -Property Name -Unique |
            Should -HaveCount 2
        }
    }

    Context 'with -Version parameter' {
        It 'should return value' {
            Find-Package -Name powershell -Version 7.4.1 |
            Should -Not -BeNullOrEmpty
        }
    }
}
