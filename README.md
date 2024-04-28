# UnityNative
Helps comunication between unity and native(Android, iOS)

## Installation
Open `Window/Package Manager` and click top left `+` button. Select `Add package from git URL`. 

Type "https://github.com/psmjazz/NativeBridge-Unity.git" and click `Add` button.

## How to use

### Container
Data storing unit.

Currently storess the following types:
|types|c#|kotlin|swift|
|---|---|---|---|
|boolean|bool|Boolean|Bool|
|32bit-integer|int|Int|Int32|
|float|float|Float|Float|
|string|string|String|String|
|byte array|byte[]|ByteArray|Data|
|other Container object|Container|Conatiner|Conatiner|

### Message
Data deliver unit.
- key : string value, identifier of message. notified messages ar deliverd to handler registerd with key.
- container : Container object

### Tag
Filters message
- MessageHandler sets Tags. It only receives message containing all set tags.
- Notifies message with Tags. messages are arrived messageHandlers which have tags all.

### MessageHandler
Notify and subscribe messages.
- Notifies message to other unity or native MessageHandler object. 
- Subscribes message from other unity or native MessageHandler object.

### Usage
Code opens native alert. Need Android/iOS implementation comunicate with this code.
```cs
public class NativeComunicator
{
    private MessageHandler messageHandler;

    public NativeComunicator()
    {
        // Create messageHandler
        messageHandler = new MessageHandler(Tag.Game);
        
        // set handler with key.
        messageHandler.SetHandler("ALERT_RESULT", OnReceive); 
    }   

    private void OnReceive(MessageHolder messageHolder)
    {
        if(messageHolder.Message.Container.TryGetValue("pressOk", out bool pressOk))
        {
            Debug.Log("user press? " + pressOk);
        }
    }

    public void OpenNativeAlert(string alertMessage)
    {
        // Create Container object and set data
        Container container = new Container();
        container.Add("alertMessage", alertMessage);
        Message message = new Message("OPEN_ALERT", container);
        // NotifyMessage 
        messageHandler.Notify(message, Tag.Native);
    }
}
```

## Build
### Android build
1. Add your own aar plugin in Assets/Plugins/Android
2. Check Project Settings → Publish Settings → Custom Main Gradle Template
3. Open Assets/Plugins/Android/mainTemplate.gradle file and add dependencies
```groovy
dependencies {
    // other dependencies
    // ...
    // other dependencies end

    implementation("androidx.core:core-ktx:1.9.0")
    implementation("com.google.protobuf:protobuf-java:3.25.3")
    implementation("com.google.protobuf:protobuf-kotlin:3.25.3")
}
```

### iOS build
#### Before unity build
1. Add your own framework int Assets/Plugins/iOS.
2. In framework inspector, check 'Add to Embeded Bin' option.
#### After unity build
1. Init pod file
```shell
pod init
```
2. Open pod file and add iOSBridgeCore dependency
```ruby
target 'Unity-iPhone' do
  # Comment the next line if you don't want to use dynamic frameworks
  use_frameworks!

  # Pods for Unity-iPhone

  target 'Unity-iPhone Tests' do
    inherit! :search_paths
    # Pods for testing
  end

end

target 'UnityFramework' do
  # Comment the next line if you don't want to use dynamic frameworks
  use_frameworks!
  pod 'iOSBridgeCore', :git => 'https://github.com/psmjazz/NativeBridge-iOS.git', :tag => '0.0.1' 
  # Pods for UnityFramework

end

post_install do |installer|
  installer.pods_project.targets.each do |target|
    target.build_configurations.each do |config|
      config.build_settings['ENABLE_BITCODE'] = 'NO'
      config.build_settings['IPHONEOS_DEPLOYMENT_TARGET'] = '12.0'
    end
  end
end

```
3. Run pod install
4. Open xcworkspace and build iOS