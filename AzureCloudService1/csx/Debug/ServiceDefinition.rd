<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="AzureCloudService1" generation="1" functional="0" release="0" Id="66227fbf-7498-4843-ad41-365d8b82c9d7" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="AzureCloudService1Group" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="ImageSharingWithCloudService:Endpoint1" protocol="https">
          <inToChannel>
            <lBChannelMoniker name="/AzureCloudService1/AzureCloudService1Group/LB:ImageSharingWithCloudService:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="ImageSharingWithCloudServiceInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/AzureCloudService1/AzureCloudService1Group/MapImageSharingWithCloudServiceInstances" />
          </maps>
        </aCS>
        <aCS name="ImageSharingWorkerRoleWithSBQueue:Microsoft.ServiceBus.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/AzureCloudService1/AzureCloudService1Group/MapImageSharingWorkerRoleWithSBQueue:Microsoft.ServiceBus.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="ImageSharingWorkerRoleWithSBQueueInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/AzureCloudService1/AzureCloudService1Group/MapImageSharingWorkerRoleWithSBQueueInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:ImageSharingWithCloudService:Endpoint1">
          <toPorts>
            <inPortMoniker name="/AzureCloudService1/AzureCloudService1Group/ImageSharingWithCloudService/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapImageSharingWithCloudServiceInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/AzureCloudService1/AzureCloudService1Group/ImageSharingWithCloudServiceInstances" />
          </setting>
        </map>
        <map name="MapImageSharingWorkerRoleWithSBQueue:Microsoft.ServiceBus.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/AzureCloudService1/AzureCloudService1Group/ImageSharingWorkerRoleWithSBQueue/Microsoft.ServiceBus.ConnectionString" />
          </setting>
        </map>
        <map name="MapImageSharingWorkerRoleWithSBQueueInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/AzureCloudService1/AzureCloudService1Group/ImageSharingWorkerRoleWithSBQueueInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="ImageSharingWithCloudService" generation="1" functional="0" release="0" software="C:\Users\Ozeh\documents\visual studio 2013\Projects\ImageSharingWithCloudService\AzureCloudService1\csx\Debug\roles\ImageSharingWithCloudService" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="https" portRanges="44302" />
            </componentports>
            <settings>
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;ImageSharingWithCloudService&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;ImageSharingWithCloudService&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;ImageSharingWorkerRoleWithSBQueue&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/AzureCloudService1/AzureCloudService1Group/ImageSharingWithCloudServiceInstances" />
            <sCSPolicyUpdateDomainMoniker name="/AzureCloudService1/AzureCloudService1Group/ImageSharingWithCloudServiceUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/AzureCloudService1/AzureCloudService1Group/ImageSharingWithCloudServiceFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="ImageSharingWorkerRoleWithSBQueue" generation="1" functional="0" release="0" software="C:\Users\Ozeh\documents\visual studio 2013\Projects\ImageSharingWithCloudService\AzureCloudService1\csx\Debug\roles\ImageSharingWorkerRoleWithSBQueue" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="-1" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="Microsoft.ServiceBus.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;ImageSharingWorkerRoleWithSBQueue&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;ImageSharingWithCloudService&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;ImageSharingWorkerRoleWithSBQueue&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/AzureCloudService1/AzureCloudService1Group/ImageSharingWorkerRoleWithSBQueueInstances" />
            <sCSPolicyUpdateDomainMoniker name="/AzureCloudService1/AzureCloudService1Group/ImageSharingWorkerRoleWithSBQueueUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/AzureCloudService1/AzureCloudService1Group/ImageSharingWorkerRoleWithSBQueueFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="ImageSharingWithCloudServiceUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="ImageSharingWorkerRoleWithSBQueueUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="ImageSharingWithCloudServiceFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="ImageSharingWorkerRoleWithSBQueueFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="ImageSharingWithCloudServiceInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="ImageSharingWorkerRoleWithSBQueueInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="75405ca5-cc5c-4289-9aa5-b34d1fbbdacb" ref="Microsoft.RedDog.Contract\ServiceContract\AzureCloudService1Contract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="1ce3b32e-ea94-4cfb-bdd8-ac60c19f45d2" ref="Microsoft.RedDog.Contract\Interface\ImageSharingWithCloudService:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/AzureCloudService1/AzureCloudService1Group/ImageSharingWithCloudService:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>