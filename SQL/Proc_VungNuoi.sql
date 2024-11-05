CREATE OR REPLACE PROCEDURE KiemTraDangNhap (
    p_username IN VARCHAR2,
    p_password IN VARCHAR2,
    p_result OUT VARCHAR2,
    p_role OUT VARCHAR2
) AS
    l_encrypted_password RAW(128);
    l_stored_password RAW(128);
    l_key RAW(8) := UTL_I18N.STRING_TO_RAW('1AQ#7T78', 'AL32UTF8'); -- khoa bi mat
    
BEGIN
    -- Ma hoa pass nguoi dung dang nhap
    l_encrypted_password := DBMS_CRYPTO.ENCRYPT(
        src => UTL_I18N.STRING_TO_RAW(p_password, 'AL32UTF8'),
        typ => DBMS_CRYPTO.DES_CBC_PKCS5,
        key => l_key
    );

    SELECT PASSWORD, ROLE INTO l_stored_password, p_role
    FROM USERS
    WHERE USERNAME = p_username;

    -- so sanh mat khau ma hoa voi mat khau trong csdl
    IF l_stored_password = l_encrypted_password THEN
        p_result := 'SUCCESS';
    ELSE
        p_result := 'FAILURE';
    END IF;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        p_result := 'FAILURE';
        p_role := 'UNKNOWN';
END;
--------------------------------------------------------------------------------
CREATE OR REPLACE PROCEDURE ThemNhanVien (
    p_NhanVienID IN VARCHAR2,
    p_TenNhanVien IN VARCHAR2,
    p_ChucVu IN VARCHAR2,
    p_SoDienThoai IN VARCHAR2,
    p_NgaySinh IN DATE,
    p_DiaChi IN VARCHAR2,
    p_NoiSinh IN VARCHAR2,
    p_MatKhau IN VARCHAR2
) AS
BEGIN
    DECLARE
        v_count NUMBER;
    BEGIN
        SELECT COUNT(*) INTO v_count FROM NhanVien WHERE NhanVienID = p_NhanVienID;

        IF v_count = 0 THEN
            INSERT INTO NhanVien (NhanVienID, TenNhanVien, ChucVu, SoDienThoai, NgaySinh, DiaChi, NoiSinh, MatKhau)
            VALUES (p_NhanVienID, p_TenNhanVien, p_ChucVu, p_SoDienThoai, p_NgaySinh, p_DiaChi, p_NoiSinh, p_MatKhau);
        END IF;
    END;
EXCEPTION
    WHEN OTHERS THEN
        NULL; 
END ThemNhanVien;

select * from NhanVien;
DESC NhanVien;
ALTER TABLE NhanVien ADD (MatKhau VARCHAR2(50));
drop procedure ThemNhanVien;

--------------------------------------------------------------------------------
CREATE OR REPLACE PROCEDURE LayThongTinNhanVien(p_Cursor OUT SYS_REFCURSOR) AS
BEGIN
    OPEN p_Cursor FOR
    SELECT * FROM NhanVien;
END;
--------------------------------------------------------------------------------
CREATE OR REPLACE PROCEDURE Dem_SoNhanVien(p_Count OUT NUMBER) AS
BEGIN
    SELECT COUNT(*) INTO p_Count FROM NhanVien;
END Dem_SoNhanVien;
--------------------------------------------------------------------------------
CREATE SEQUENCE SEQ_KHACHHANGID 
  START WITH 1 
  INCREMENT BY 1 
  NOCACHE 
  NOCYCLE;


CREATE OR REPLACE PROCEDURE TAO_NGUOIDUNG( 
    p_username IN VARCHAR2,
    p_password IN VARCHAR2,
    p_tenkh IN VARCHAR2,
    p_diachi IN VARCHAR2,
    p_sodienthoai IN VARCHAR2,
    p_email IN VARCHAR2
)
AS
    v_KhachHangID VARCHAR2(10);
    v_exists INT;
    v_role VARCHAR2(20) := 'USER';
    v_short_username VARCHAR2(30);
BEGIN
    v_short_username := SUBSTR(p_username, 1, 30);

    -- Ki?m tra xem username ?? t?n t?i ch?a
    KiemTraUsernameTonTai(v_short_username, v_exists);
    IF v_exists > 0 THEN
        RAISE_APPLICATION_ERROR(-20001, 'Username ?? t?n t?i!');
    END IF;

    -- T?o m? kh?ch h?ng duy nh?t
    LOOP
        SELECT 'KH' || LPAD(SEQ_KHACHHANGID.NEXTVAL, 2, '0') INTO v_KhachHangID FROM DUAL;
        SELECT COUNT(*) INTO v_exists FROM KHACHHANG WHERE KHACHHANGID = v_KhachHangID;
        EXIT WHEN v_exists = 0;
    END LOOP;

    -- T?o ng??i d?ng trong Oracle
    EXECUTE IMMEDIATE 'CREATE USER "' || v_short_username || '" IDENTIFIED BY "' || p_password || '"';

    -- Thay ??i profile c?a ng??i d?ng th?nh Nguoi_Dung1
    EXECUTE IMMEDIATE 'ALTER USER "' || v_short_username || '" PROFILE Nguoi_Dung1';

    -- C?p quy?n cho ng??i d?ng
    EXECUTE IMMEDIATE 'GRANT CONNECT TO "' || v_short_username || '"';
    EXECUTE IMMEDIATE 'GRANT SELECT ON KHACHHANG TO "' || v_short_username || '"';

    -- Th?m b?n ghi v?o b?ng USERS
    INSERT INTO USERS (USERNAME, PASSWORD, ROLE, KHACHHANGID)
    VALUES (v_short_username, p_password, v_role, v_KhachHangID);

    -- Th?m b?n ghi v?o b?ng KHACHHANG
    INSERT INTO KHACHHANG (KHACHHANGID, TENKHACHHANG, DIACHI, SODIENTHOAI, EMAIL)
    VALUES (v_KhachHangID, p_tenkh, p_diachi, p_sodienthoai, p_email);

    -- Commit thay ??i
    COMMIT;

EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE_APPLICATION_ERROR(-20002, 'L?i t?o ng??i d?ng: ' || SQLERRM || ' | Chi ti?t: ' || DBMS_UTILITY.FORMAT_CALL_STACK);
END;
/





select * from users
UPDATE Users
SET ROLE = 'ADMIN'
WHERE username = 'watthe8';

select * from khachhang
--------------------------------------------------------------------------------
CREATE OR REPLACE PROCEDURE KiemtraSession(
    p_username IN VARCHAR2,
    p_sessionID OUT VARCHAR2
) AS
BEGIN
    SELECT LastSessionID
    INTO p_sessionID
    FROM USERS
    WHERE Username = p_username;
END;
/
--------------------------------------------------------------------------------
CREATE OR REPLACE PROCEDURE DondepSession(
    p_username IN VARCHAR2
) AS
BEGIN
    UPDATE USERS
    SET LastSessionID = NULL
    WHERE Username = p_username;
END;
/
--------------------------------------------------------------------------------
CREATE OR REPLACE PROCEDURE Capnhat_SessionDangNhap(
    p_username IN VARCHAR2,
    p_sessionID IN VARCHAR2
) AS
BEGIN
    UPDATE USERS
    SET LastSessionID = p_sessionID
    WHERE Username = p_username;
END;
/
--------------------------------------------------------------------------------
CREATE OR REPLACE PROCEDURE KiemTraUsernameTonTai (
    p_username IN VARCHAR2,
    p_exists OUT INT
) AS
BEGIN
    SELECT COUNT(*)
    INTO p_exists
    FROM USERS
    WHERE Username = p_username;
END;
/
----------------XAC THUC NGUOI DUNG----------------------------------------------------------------
CREATE OR REPLACE PROCEDURE Kiem_TraDangNhap(
    p_username IN VARCHAR2,
    p_password IN VARCHAR2,  -- nhan mat khau ma hoa
    p_result OUT VARCHAR2,
    p_role OUT VARCHAR2
) AS
    v_password_db VARCHAR2(100);  -- lay pass tu database
BEGIN
    -- tim pass tu username trong database
    SELECT PASSWORD, ROLE
    INTO v_password_db, p_role
    FROM USERS
    WHERE USERNAME = p_username;

    -- So sanh
    IF v_password_db = p_password THEN
        p_result := 'SUCCESS';  -- login thanh cong
    ELSE
        p_result := 'FAILURE';  
    END IF;

EXCEPTION
    
    WHEN NO_DATA_FOUND THEN
        p_result := 'FAILURE';
        p_role := NULL;  -- neu ko co du lieu thi role la null
END;

--------------------------------------------------------------------------------
grant execute on DBMS_RLS to public;
DESCRIBE DBMS_RLS.ADD_POLICY;
SELECT * FROM DBA_SYS_PRIVS WHERE GRANTEE = 'VUNGNUOI30';

GRANT EXECUTE ON DBMS_RLS TO VUNGNUOI30;
GRANT CREATE ANY POLICY TO VUNGNUOI30;

SELECT * FROM ALL_OBJECTS WHERE OBJECT_NAME = 'DBMS_RLS';

GRANT EXECUTE ON DBMS_RLS TO VUNGNUOI30;

------------TAO VPD-------------------------------------------------------------
CREATE OR REPLACE PROCEDURE DuLieuVPD (cur OUT SYS_REFCURSOR) AS
BEGIN
    OPEN cur FOR
    SELECT 
        TenKhachHang, 
        DiaChi, 
        SoDienThoai 
    FROM 
        KhachHang; 
END;
/
BEGIN
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'VUNGNUOI30',    -- Schema containing the table
        object_name     => 'KHACHHANG',     -- Table name
        policy_name     => 'vpd_khachhang_policy', -- Policy name
        function_schema => 'VUNGNUOI30',    -- Schema containing the function
        function_name   => 'XuLyVPD',       -- Function name
        statement_types => 'SELECT',        -- Statement types (SELECT)
        update_check    => FALSE,           -- Optional parameter, set to FALSE if not needed
        enable          => TRUE,            -- Optional parameter, set to TRUE to enable the policy
        static_policy   => FALSE            -- Optional parameter, set to FALSE for dynamic policy
    );
END;
/
--------------------------------------------------------------------------------
CREATE OR REPLACE FUNCTION XuLyVPD (schema_name IN VARCHAR2, object_name IN VARCHAR2)
RETURN VARCHAR2 AS
    v_role VARCHAR2(20);
BEGIN
    SELECT role INTO v_role
    FROM USERS
    WHERE username = SYS_CONTEXT('USERENV', 'SESSION_USER');

    IF v_role = 'ADMIN' THEN
        RETURN '1=0'; --an cot
    ELSIF v_role = 'SUPER_ADMIN' THEN
        RETURN NULL; -- ko an cot
    ELSE
        RETURN '1=0'; -- roll khac ko xem dc
    END IF;
END;
/



GRANT EXECUTE ON DBMS_RLS TO VUNGNUOI;

SELECT * 
FROM DBA_TAB_PRIVS 
WHERE GRANTEE = 'VUNGNUOI' 
AND TABLE_NAME = 'DBMS_RLS' 
AND PRIVILEGE = 'EXECUTE';

SELECT NAME, CON_ID
FROM V$PDBS
WHERE NAME = 'VUNGNUOI30';



----------------------------------------------------------------------------------------
BEGIN
    FOR user_record IN (SELECT username FROM USERS WHERE ROLE = 'ADMIN') LOOP
        EXECUTE IMMEDIATE 'ALTER USER "' || user_record.username || '" PROFILE Quan_TriVien';
    END LOOP;
END;
/

select * from KhachHang


select * from users

CREATE USER watthe8 IDENTIFIED BY "741C58C6680AF67F";

SELECT username, profile
FROM dba_users
WHERE username = 'watthe8';


BEGIN
    FOR user_record IN (SELECT username FROM USERS) LOOP
        EXECUTE IMMEDIATE 'ALTER USER "' || user_record.username || '" PROFILE Nguoi_Dung1';
    END LOOP;
END;
/
--------------------------------------------------------------------------------
CREATE OR REPLACE PROCEDURE XoaTaiKhoan(p_username IN VARCHAR2) IS
BEGIN
    DELETE FROM USERS WHERE USERNAME = p_username;
    COMMIT;
END;
/
--------------------------------------------------------------------------------
ALTER SESSION SET CONTAINER = VUNGNUOI10;
GRANT SELECT ON SYS.DBA_PROFILES TO VUNGNUOI10;

CREATE OR REPLACE PROCEDURE GetResourceNames (p_Cursor OUT SYS_REFCURSOR) AS
BEGIN
    OPEN p_Cursor FOR
    SELECT RESOURCE_NAME FROM DBA_PROFILES;
END;



--------------------------------------------------------------------------------
CREATE OR REPLACE PROCEDURE CapNhatThongTinNguoiDung(
    p_userId       IN VARCHAR2,
    p_ten          IN VARCHAR2,
    p_soDienThoai  IN VARCHAR2,
    p_ngaySinh     IN DATE,
    p_matKhau      IN VARCHAR2,
    p_diaChi       IN VARCHAR2,
    p_noiSinh      IN VARCHAR2,
    p_resourceName IN VARCHAR2,
    p_quantity     IN NUMBER
) AS
    v_sql VARCHAR2(4000);
BEGIN
    -- C?p nh?t th?ng tin c? b?n c?a nh?n vi?n trong b?ng NHANVIEN
    UPDATE NHANVIEN
    SET 
        TENNHANVIEN = p_ten,
        SODIENTHOAI = p_soDienThoai,
        NGAYSINH = p_ngaySinh,
        MATKHAU = p_matKhau,
        DIACHI = p_diaChi,
        NOISINH = p_noiSinh
    WHERE NHANVIENID = p_userId;

    -- X?y d?ng c?u l?nh ALTER USER cho thu?c t?nh c? th?
    IF p_resourceName IN ('FAILED_LOGIN_ATTEMPTS', 'PASSWORD_LIFE_TIME', 'PASSWORD_REUSE_TIME', 'PASSWORD_LOCK_TIME', 'INACTIVE_ACCOUNT_TIME') THEN
        v_sql := 'ALTER PROFILE QUAN_TRIVIEN LIMIT ' || p_resourceName || ' ' || p_quantity;
    ELSE
        RAISE_APPLICATION_ERROR(-20003, 'Thu?c t?nh ' || p_resourceName || ' kh?ng h?p l? ho?c kh?ng th? c?p nh?t tr?c ti?p cho ng??i d?ng.');
    END IF;
    
    -- Th?c hi?n c?u l?nh ALTER PROFILE n?u c?n
    EXECUTE IMMEDIATE v_sql;

    -- X?c nh?n th?nh c?ng
    COMMIT;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RAISE_APPLICATION_ERROR(-20002, 'Kh?ng t?m th?y nh?n vi?n v?i ID: ' || p_userId);
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE_APPLICATION_ERROR(-20001, 'L?i khi c?p nh?t th?ng tin ng??i d?ng: ' || SQLERRM);
END CapNhatThongTinNguoiDung;



--------------------------------------------------------------------------------















--step1
SHOW CON_NAME;

--step2
SELECT NAME, OPEN_MODE FROM V$PDBS;

--step3
ALTER SESSION SET CONTAINER = XEPDB1;

--step4
create user VUNGNUOI identified by vungnuoi;

--step5
GRANT CONNECT, RESOURCE TO VUNGNUOI;

--step6
grant dba to VUNGNUOI;

--step7
SELECT NAME, CON_ID, OPEN_MODE FROM V$PDBS;
SELECT name, pdb FROM v$services;

--------------------------------------------------------------------------------
SELECT * FROM DBA_SYS_PRIVS 
WHERE GRANTEE = 'C##VUNGNUOI';

grant create user to C##VUNGNUOI


create user kh2 identified by kh2;
alter user kh2 profile default
grant connect to kh2
grant select on khachhang to kh2

create user C##kh1 identified by kh1





SELECT username, profile
FROM dba_users
WHERE username = 'VUNGNUOI30';






create user C##taikhoan1 identified by taikhoan1;
grant connect to C##taikhoan1;
grant select on khachhang to C##taikhoan1;


CREATE PROFILE C##my_profile 
LIMIT 
  IDLE_TIME 2 
  PASSWORD_LIFE_TIME 90 
  FAILED_LOGIN_ATTEMPTS 3;


ALTER USER C##taikhoan1 PROFILE C##my_profile;