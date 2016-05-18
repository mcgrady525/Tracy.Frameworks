使用方法：
<configuration>
  <connectionStrings configSource="VConfigs\Debug\connectionStrings.config"/>
  <appSettings configSource="VConfigs\Debug\appSettings.config" />
    
  <system.serviceModel>
    <extensions  configSource="VConfigs\Debug\WCF.extensions.config" />
    <services    configSource="VConfigs\Debug\WCF.services.config" />
    <behaviors   configSource="VConfigs\Debug\WCF.behaviors.config" />
    <bindings    configSource="VConfigs\Debug\WCF.bindings.config" />
    <client      configSource="VConfigs\Debug\WCF.client.config" />
    <serviceHostingEnvironment aspNetCompatibilityEnabled="true" multipleSiteBindingsEnabled="true" />
  </system.serviceModel>
</configuration>