SELECT logins FROM v$instance;

ALTER SESSION SET CONTAINER = CDB$ROOT;

SELECT name, cause, type, message, status, action 
FROM PDB_PLUG_IN_VIOLATIONS 
WHERE type = 'ERROR' AND status = 'PENDING';

DELETE FROM PDB_PLUG_IN_VIOLATIONS WHERE status = 'PENDING';

ALTER SESSION SET CONTAINER = VUNGNUOI;

--2 CAU LENH DUOI NAY CHI DUOC DUNG DE TEST USER CO DANG NHAP VAO DATABASE DUOC KHONG
ALTER SYSTEM DISABLE RESTRICTED SESSION;
ALTER SYSTEM ENABLE RESTRICTED SESSION;

--Kiem tra
SELECT name, open_mode FROM v$pdbs;

--TAO TABLESPACE
CREATE TABLESPACE VUNGNUOI10_TABLESPACE
  DATAFILE 'E:\hufi\DoAnChuyenNganh\Project\DOANCHUYENNGANH\ORCL\VUNGNUOI_TABLESPACE.dbf' 
  SIZE 100M 
  AUTOEXTEND ON 
  NEXT 10M 
  MAXSIZE UNLIMITED;
--KIEM TRA 
SELECT TABLESPACE_NAME, STATUS FROM DBA_TABLESPACES WHERE TABLESPACE_NAME = 'VUNGNUOI10_TABLESPACE';


--b1
CREATE PLUGGABLE DATABASE VUNGNUOI10
  ADMIN USER VUNGNUOI10 IDENTIFIED BY vungnuoi10
  FILE_NAME_CONVERT = ('E:\hufi\oracle\21c\oradata\XE\pdbseed', 'E:\hufi\DoAnChuyenNganh\Project\DOANCHUYENNGANH\ORCL\VUNGNUOI10');
--b2
ALTER PLUGGABLE DATABASE VUNGNUOI10 OPEN;
--b3 cap quyen
ALTER SESSION SET CONTAINER = VUNGNUOI10;

ALTER USER VUNGNUOI10 QUOTA UNLIMITED ON VUNGNUOI10_TABLESPACE;

GRANT ALL PRIVILEGES TO VUNGNUOI10;

GRANT CREATE SESSION TO VUNGNUOI10;
GRANT CREATE USER TO VUNGNUOI10;
GRANT DBA TO VUNGNUOI10; 
GRANT UNLIMITED TABLESPACE TO VUNGNUOI10;
ALTER USER VUNGNUOI10 QUOTA UNLIMITED ON USERS; 
GRANT EXECUTE ON DBMS_CRYPTO TO VUNGNUOI10;
--------------------------------------------------------------------------------
CREATE PROFILE vungnuoiAD LIMIT PASSWORD_LIFE_TIME UNLIMITED;
ALTER USER VUNGNUOI30 PROFILE vungnuoiAD;