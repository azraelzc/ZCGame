@SET DBPath="C:\Program Files\MySQL\MySQL Server 5.5\bin"
@SET PWD=123456
@SET DBName=zcgame
cd /d %DBPath%
echo "11111111"
mysql -u root -p%PWD% %DBName% --execute=
"
	drop table test if exists;
	create table test(
	id int(10) unsigned NOT NULL AUTO_INCREMENT,
	name varchar(10) NOT NULL DEFAULT '',
);
"

--execute=""
@pause