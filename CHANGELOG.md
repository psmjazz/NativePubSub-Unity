# Changelog

## [Unreleased]

### Added

- Envelop struct added. Envelop has a Message
- EnvelopeHolder struct added. Holds Envelope struct internally.
- Add MatchTag to Receivable
- Add SetReceivingRule to Receivable
- Add SetBasePublishingTag to ReceivablePublisher

### Changed

- Renmae unity package NativeBridge to NativePubSub
- Rename namespace PJ.Native.Messenger to PJ.Native.PubSub
- Rename Folder Runtime/Scripts/Messenger to Runtime/Scripts/PubSub
- Tags are now synced between unity and native.

### Deprecated

### Removed

- Delete MessageCollector.
- Delete MessageHandler.

### Fixed

### Security

## [0.0.1] - 2024-02-29

### Added
- Initial release