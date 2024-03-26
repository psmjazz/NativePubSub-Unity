# Changelog

## [Unreleased]

### Added

- Envelop struct added. Envelop has a Message
- Add MatchTag to Receivable
- Add SetTagRules to Receivable
- Add SetBaseTag to ReceivablePublisher

### Changed

- Renmae unity package NativeBridge to NativePubSub
- Rename namespace PJ.Native.Messenger to PJ.Native.PubSub
- Rename Folder Runtime/Scripts/Messenger to Runtime/Scripts/PubSub
- Rename MessageHandler's method
- Rename MessageHandler to Messenger.
- Rename MessageHolder to Channel
- Rename MessageHolder.GiveBack to Channel.Reply

### Deprecated

### Removed

- Delete MessageCollector.

### Fixed

### Security

## [0.0.1] - 2024-02-29

### Added
- Initial release