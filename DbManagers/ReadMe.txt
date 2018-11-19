
DbManagers：数据库处理




时间：2016/02/01
版本：1.0.1.10201
修改：
	1.AdvancedSetupManager中标识_isTh：
	  _isTh=true时是重庆农商行版本，6位3000个分机号（100000--102999）
	  _isTh=false时是基本版本，5位1000个分机号（10000--10999）

时间：2015/10/21
版本：1.0.0.11021
修改：
	1.转移表转移分机号改存分机id
	2.转移设置表转移分机号改存分机id

时间：2015/04/17
版本：1.0.0.10417
修改：
	1.增加UserManager，与数据库交互
	2.优化与数据库交互逻辑，容错处理

时间：2015/04/21
版本：1.0.0.10421
修改：
	1.优化处理，解决数据库错误链接导致系统崩溃问题
	

测试数据插入：

INSERT INTO `ipvt_deviceinfotable` VALUES ('70', '13070', '1', 'IP-Phone', null, null, null, '5060', '192.168.4.70', null, null, null, null, null, null, null, '2');
INSERT INTO `ipvt_deviceinfotable` VALUES ('71', '13071', '0', 'IP-Center', null, null, null, '5060', '192.168.4.71', null, null, null, null, null, null, null, '2');
INSERT INTO `ipvt_deviceinfotable` VALUES ('72', '13072', '0', 'CenterDevice', null, null, null, '5060', '192.168.4.72', null, null, null, null, null, null, null, '2');			
INSERT INTO `ipvt_deviceinfotable` VALUES ('73', '13073', '1', 'split-type_audio', null, null, null, '5060', '192.168.4.73', '2.0.5_53', 'TSS-IAIPZ102-V1.0', null, null, null, null, null, '2');
INSERT INTO `ipvt_deviceinfotable` VALUES ('74', '13074', '1', 'IP-Phone', null, null, null, '5060', '192.168.4.74', null, null, null, null, null, null, null, '2');
INSERT INTO `ipvt_deviceinfotable` VALUES ('75', '13075', '0', 'IP-Center', null, null, null, '5060', '192.168.4.75', null, null, null, null, null, null, null, '2');
INSERT INTO `ipvt_deviceinfotable` VALUES ('76', '13076', '0', 'CenterDevice', null, null, null, '5060', '192.168.4.76', null, null, null, null, null, null, null, '2');
INSERT INTO `ipvt_deviceinfotable` VALUES ('77', '13077', '1', 'split-type_audio', null, null, null, '5060', '192.168.4.77', '2.0.5_53', 'TSS-IAIPZ102-V1.0', null, null, null, null, null, '2');
INSERT INTO `ipvt_deviceinfotable` VALUES ('78', '13078', '1', 'IP-Phone', null, null, null, '5060', '192.168.4.78', null, null, null, null, null, null, null, '2');
INSERT INTO `ipvt_deviceinfotable` VALUES ('79', '13079', '0', 'IP-Center', null, null, null, '5060', '192.168.4.79', null, null, null, null, null, null, null, '2');

Update `ipvt_extensionmessagetable` set DeviceIP='192.168.4.70',CurrentState=1,StateID=2 where ExtensionID='13070';
Update `ipvt_extensionmessagetable` set DeviceIP='192.168.4.71',CurrentState=1,StateID=2 where ExtensionID='13071';
Update `ipvt_extensionmessagetable` set DeviceIP='192.168.4.72',CurrentState=1,StateID=2 where ExtensionID='13072';
Update `ipvt_extensionmessagetable` set DeviceIP='192.168.4.73',CurrentState=1,StateID=2 where ExtensionID='13073';
Update `ipvt_extensionmessagetable` set DeviceIP='192.168.4.74',CurrentState=1,StateID=2 where ExtensionID='13074';
Update `ipvt_extensionmessagetable` set DeviceIP='192.168.4.75',CurrentState=1,StateID=2 where ExtensionID='13075';
Update `ipvt_extensionmessagetable` set DeviceIP='192.168.4.76',CurrentState=1,StateID=2 where ExtensionID='13076';
Update `ipvt_extensionmessagetable` set DeviceIP='192.168.4.77',CurrentState=1,StateID=2 where ExtensionID='13077';
Update `ipvt_extensionmessagetable` set DeviceIP='192.168.4.78',CurrentState=1,StateID=2 where ExtensionID='13078';
Update `ipvt_extensionmessagetable` set DeviceIP='192.168.4.79',CurrentState=1,StateID=2 where ExtensionID='13079';