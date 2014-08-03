----------------------------------------------
-- Export file for user USER1               --
-- Created by Romeo on 03.08.2014, 20:02:43 --
----------------------------------------------

set define off
spool ddl.log

prompt
prompt Creating table FOLDERS
prompt ======================
prompt
create table USER1.FOLDERS
(
  id             NUMBER not null,
  name           VARCHAR2(500) not null,
  mainattributes NUMBER(1) default 0 not null,
  datecreate     DATE default current_date not null,
  dateedit       DATE,
  parentid       NUMBER,
  nestedlevel    NUMBER default -1 not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
comment on column USER1.FOLDERS.name
  is 'Наименование';
comment on column USER1.FOLDERS.mainattributes
  is 'Основные атрибуты (битовый флаг)';
comment on column USER1.FOLDERS.datecreate
  is 'Дата создания';
comment on column USER1.FOLDERS.dateedit
  is 'Дата изменеия';
comment on column USER1.FOLDERS.parentid
  is 'ID родителя';
comment on column USER1.FOLDERS.nestedlevel
  is 'Ограничение уровня вложенности (-1 - нет ограничения)';
alter table USER1.FOLDERS
  add constraint PK_FOLDERS primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table USER1.FOLDERS
  add constraint UK_FOLDERS unique (NAME, PARENTID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table USER1.FOLDERS
  add constraint PK_FOLDERS_FOLDERS foreign key (PARENTID)
  references USER1.FOLDERS (ID);

prompt
prompt Creating table FILES
prompt ====================
prompt
create table USER1.FILES
(
  id             NUMBER not null,
  name           VARCHAR2(500) not null,
  sz             NUMBER default 0 not null,
  mainattributes NUMBER(1) default 0 not null,
  datecreate     DATE default current_date not null,
  dateedit       DATE,
  folderid       NUMBER not null,
  data           BLOB
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
comment on column USER1.FILES.name
  is 'Наменование';
comment on column USER1.FILES.sz
  is 'Размер, байт';
comment on column USER1.FILES.mainattributes
  is 'Основные атрибуты (битовый флаг)';
comment on column USER1.FILES.datecreate
  is 'Дата создания';
comment on column USER1.FILES.dateedit
  is 'Дата редактирования';
comment on column USER1.FILES.folderid
  is 'ID каталога';
comment on column USER1.FILES.data
  is 'Содержимое файла';
alter table USER1.FILES
  add constraint PK_FILES primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table USER1.FILES
  add constraint UK_FILES unique (NAME, FOLDERID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table USER1.FILES
  add constraint FK_FILES_FOLDERS foreign key (FOLDERID)
  references USER1.FOLDERS (ID);

prompt
prompt Creating table FILE_ATTRIBUTES
prompt ==============================
prompt
create table USER1.FILE_ATTRIBUTES
(
  id          NUMBER not null,
  name        VARCHAR2(500) not null,
  numbervalue NUMBER,
  datevalue   DATE,
  stringvalue VARCHAR2(4000),
  binaryvalue BLOB,
  fileid      NUMBER not null,
  datatype    NUMBER(1) default 0 not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
comment on column USER1.FILE_ATTRIBUTES.name
  is 'Наименование';
comment on column USER1.FILE_ATTRIBUTES.fileid
  is 'ID файла';
comment on column USER1.FILE_ATTRIBUTES.datatype
  is 'тип аттрибута (0 - число, 1 - дата/время, 2 - строка, 3 - бинарный файл )';
alter table USER1.FILE_ATTRIBUTES
  add constraint PK_FILE_ATTRIBUTES primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table USER1.FILE_ATTRIBUTES
  add constraint UK_FILE_ATTRIBUTES unique (FILEID, NAME)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table USER1.FILE_ATTRIBUTES
  add constraint FK_FILE_ATTRIBUTES_FILES foreign key (FILEID)
  references USER1.FILES (ID) on delete cascade;

prompt
prompt Creating table FOLDER_ATTRIBUTES
prompt ================================
prompt
create table USER1.FOLDER_ATTRIBUTES
(
  id          NUMBER not null,
  name        VARCHAR2(500) not null,
  numbervalue NUMBER,
  datevalue   DATE,
  stringvalue VARCHAR2(4000),
  binaryvalue BLOB,
  folderid    NUMBER not null,
  datatype    NUMBER(1) default 0 not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
comment on column USER1.FOLDER_ATTRIBUTES.name
  is 'Наименование';
comment on column USER1.FOLDER_ATTRIBUTES.folderid
  is 'ID каталога';
comment on column USER1.FOLDER_ATTRIBUTES.datatype
  is 'тип аттрибута (0 - число, 1 - дата/время, 2 - строка, 3 - бинарный файл )';
alter table USER1.FOLDER_ATTRIBUTES
  add constraint PK_FOLDER_ATTRIBUTES primary key (ID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table USER1.FOLDER_ATTRIBUTES
  add constraint UK_FOLDER_ATTRIBUTES unique (NAME, FOLDERID)
  using index 
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table USER1.FOLDER_ATTRIBUTES
  add constraint FK_FOLDER_ATTRIBUTES_FOLDERS foreign key (FOLDERID)
  references USER1.FOLDERS (ID) on delete cascade;

prompt
prompt Creating sequence FILEATTRIBUTES_SEQ
prompt ====================================
prompt
create sequence USER1.FILEATTRIBUTES_SEQ
minvalue 1
maxvalue 9999999999999999999999999999
start with 21
increment by 1
cache 20;

prompt
prompt Creating sequence FILES_SEQ
prompt ===========================
prompt
create sequence USER1.FILES_SEQ
minvalue 1
maxvalue 9999999999999999999999999999
start with 41
increment by 1
cache 20;

prompt
prompt Creating sequence FILES_SEQ1
prompt ============================
prompt
create sequence USER1.FILES_SEQ1
minvalue 1
maxvalue 9999999999999999999999999999
start with 1
increment by 1
cache 20;

prompt
prompt Creating sequence FOLDERATTRIBUTES_SEQ
prompt ======================================
prompt
create sequence USER1.FOLDERATTRIBUTES_SEQ
minvalue 1
maxvalue 9999999999999999999999999999
start with 21
increment by 1
cache 20;

prompt
prompt Creating sequence FOLDERS_SEQ
prompt =============================
prompt
create sequence USER1.FOLDERS_SEQ
minvalue 1
maxvalue 9999999999999999999999999999
start with 41
increment by 1
cache 20;

prompt
prompt Creating package FS
prompt ===================
prompt
create or replace package user1.FS is

  -- Author  : ROMEO
  -- Created : 03.08.2014 11:57:06
  -- Purpose : File System
  
  /*
  Получить размер файла
  pID - ID файла
  */  
  Function GetFileSize(pID IN NUMBER)   RETURN NUMBER;
  
  /*
  Получить размер каталога (с вложенными файлами и каталогами)
  pID - ID каталога
  */  
  Function GetFolderSize(pID IN NUMBER) RETURN NUMBER;

  /*
  Удалить файл
  pID - ID файла
  */    
  Procedure DeleteFile(pID IN NUMBER);
  
  /*
  Удалить файл
  pFOLDERID - ID каталога, в котором расположен файл,
  pNAME - наименование файла
  */    
  Procedure DeleteFile(pFOLDERID IN NUMBER, pNAME IN VARCHAR2);
  
  /*
  Удалить каталог
  pID - ID каталога,
  pWithChildren - (0, 1) - если 1, удалится вместе с вложенными файлами и каталогами  
  */
  Procedure DeleteFolder(pID IN NUMBER, pWithChildren IN NUMBER DEFAULT 0);
  
  /*
  Удалить каталог
  pPARENTID - ID родителя каталога,
  pNAME - наименование каталога
  pWithChildren - (0, 1) - если 1, удалится вместе с вложенными файлами и каталогами  
  */
  Procedure DeleteFolder(pPARENTID IN NUMBER, pNAME IN VARCHAR2, pWithChildren IN NUMBER DEFAULT 0);
  
  /*
  Удалить аттрибут файла
  pFILEID - ID файла
  pNAME - наименование аттрибута  
  */
  Procedure DeleteFileAttribute(pFILEID IN NUMBER, pNAME VARCHAR2);

  /*
  Удалить аттрибут каталога
  pFOLDERID - ID каталога
  pNAME - наименование аттрибута  
  */
  Procedure DeleteFolderAttribute(pFOLDERID IN NUMBER, pNAME VARCHAR2);

  /*
  Получить все аттрибуты файла
  pFileID - ID файла
  */  
  Procedure GetFileAttributes(pFILEID IN NUMBER);
  
  /*
  Получить все аттрибуты каталога
  pFolderID - ID каталога
  */
  Procedure GetFolderAttributes(pFOLDERID IN NUMBER);
  
  /*
  Получить все файлы каталога
  pFolderID - ID каталога
  */
  Procedure GetFiles(pFOLDERID IN NUMBER);
  
  /*
  Получить все вложенные каталоги (и файлы каталогов)
  pID - ID каталога, если не указан - сторится вся схема
  */
  Procedure GetFolders(pID IN NUMBER DEFAULT NULL);
  
  /*
  Создать/изменить файл
  pID - ID файла,
  pNAME - наименование файла,
  pFOLDERID - ID каталога,
  pMAINATTRIBUTES - основные аттрибуты,
  pSZ - размер (в байтах),
  pData - содержимое файла
  */
  Procedure UpdateFile(pID IN NUMBER, pNAME VARCHAR2, pFOLDERID NUMBER, pMAINATTRIBUTES NUMBER DEFAULT 0, pSZ NUMBER DEFAULT 0, pDATA BLOB DEFAULT NULL);
    
  /*
  Создать/изменить каталог
  pID - ID каталог,
  pNAME - наименование каталога,
  pMAINATTRIBUTES - основные аттрибуты,
  pPARENTID - ID род. каталога (если есть)
  */
  Procedure UpdateFolder(pID IN NUMBER, pNAME VARCHAR2, pMAINATTRIBUTES NUMBER DEFAULT 0, pNESTEDLEVEL NUMBER DEFAULT -1, pPARENTID NUMBER DEFAULT NULL);     
  
  /*
  Создать/изменить аттрибут файла
  pFILEID - ID файла
  pNAME - наименование аттрибута,
  pDATATYPE - тип аттрибуты (0 - числовой, 1 - дата, 2 - строковый, 3 - бинарный),
  pNUMBERVALUE - числовое значение,
  pDATEVALUE - дата значение,
  pSTRINGVALUE - строковое значение,
  pBINARYVALUE - бинарное значение
  */
  Procedure UpdateFileAttribute(pFILEID IN NUMBER, pNAME VARCHAR2, pDATATYPE IN NUMBER, pNUMBERVALUE IN NUMBER, pDATEVALUE IN DATE, pSTRINGVALUE IN VARCHAR2, pBINARYVALUE IN BLOB);

  /*
  Создать/изменить каталога
  pFOLDERD - ID каталога
  pNAME - наименование аттрибута,
  pDATATYPE - тип аттрибуты (0 - числовой, 1 - дата, 2 - строковый, 3 - бинарный),
  pNUMBERVALUE - числовое значение,
  pDATEVALUE - дата значение,
  pSTRINGVALUE - строковое значение,
  pBINARYVALUE - бинарное значение
  */
  Procedure UpdateFolderAttribute(pFOLDERID IN NUMBER, pNAME VARCHAR2, pDATATYPE IN NUMBER, pNUMBERVALUE IN NUMBER, pDATEVALUE IN DATE, pSTRINGVALUE IN VARCHAR2, pBINARYVALUE IN BLOB);
end FS;
/

prompt
prompt Creating package body FS
prompt ========================
prompt
create or replace package body user1.FS is

  -- GetFileSize
  Function GetFileSize(pID IN NUMBER) RETURN NUMBER IS
    sz  NUMBER := 0;
  BEGIN
    
    SELECT f.SZ INTO sz FROM FILES f WHERE f.ID = pID;
    
    EXCEPTION
      WHEN others THEN dbms_output.put_line('file not found');
    
    RETURN sz;
    
  END GetFileSize;

  -- GetFolderSize
  Function GetFolderSize(pID IN NUMBER) RETURN NUMBER IS
    sz  NUMBER;
  BEGIN
    -- поиск каталогов
    WITH cte(id)
    AS
    (
         SELECT id FROM FOLDERS 
         WHERE id = pID
         UNION ALL
         SELECT f.id FROM FOLDERS f
         JOIN cte ON f.parentid = cte.id
    )
    SELECT SUM(SZ) INTO sz from FILES JOIN cte ON FOLDERID = cte.id;

    RETURN NVL(sz, 0);
    
  END GetFolderSize;

  -- DeleteFile
  Procedure DeleteFile(pID IN NUMBER) IS
  BEGIN
    DELETE FROM FILES
    WHERE ID = pID; 
    
    COMMIT;
  
  END DeleteFile;

  -- DeleteFile
  Procedure DeleteFile(pFOLDERID IN NUMBER, pNAME IN VARCHAR2) IS
  BEGIN
    DELETE FROM FILES
    WHERE FOLDERID = pFOLDERID AND NAME = pNAME; 
    
    COMMIT;   
      
  END DeleteFile;
  
  -- DeleteFileAttribute
  Procedure DeleteFileAttribute(pFileID IN NUMBER, pNAME VARCHAR2) IS
  BEGIN
    DELETE FROM FILE_ATTRIBUTES
    WHERE FILEID = pFILEID AND NAME = pNAME; 
    
    COMMIT;  
      
  END DeleteFileAttribute;
  
  -- DeleteFolder
  Procedure DeleteFolder(pID IN NUMBER, pWithChildren IN NUMBER DEFAULT 0) IS
    v_id    FOLDERS.ID%TYPE;

    CURSOR c IS
      SELECT id FROM FOLDERS 
      START WITH ID = pID
      CONNECT BY PRIOR ID = PARENTID
      ORDER BY level DESC;
  BEGIN
    -- удалить все файлы каталога
    DELETE FROM FILES
    WHERE FOLDERID = pID;
    
    -- удалить все вложенные каталоги  
    IF pWithChildren = 1 THEN
      
      OPEN c; 
      
        LOOP
            FETCH c INTO v_id;
            EXIT WHEN c%NOTFOUND;

            fs.DeleteFolder(v_id);
        
        END LOOP;
	
      	CLOSE c;      
      
    END IF;                
  
    -- удалить сам каталог
    DELETE FROM FOLDERS
    WHERE ID = pID;
  
    COMMIT;
     
  END DeleteFolder;
  
  -- DeleteFolder
  Procedure DeleteFolder(pPARENTID IN NUMBER, pNAME IN VARCHAR2, pWithChildren IN NUMBER DEFAULT 0) IS
    v_id    FOLDERS.ID%TYPE;
  BEGIN
    SELECT f.ID INTO v_id FROM FOLDERS f WHERE f.PARENTID = pPARENTID AND f.NAME = pNAME;
    FS.DeleteFolder(v_id, pWithchildren);      
  END DeleteFolder;
  
  -- DeleteFolderAttribute
  Procedure DeleteFolderAttribute(pFOLDERID IN NUMBER, pNAME VARCHAR2) IS
  BEGIN
    DELETE FROM FOLDER_ATTRIBUTES
    WHERE FOLDERID = pFOLDERID AND NAME = pNAME; 
    
    COMMIT;    
      
  END DeleteFolderAttribute;
  
  -- GetFileAttributes
  Procedure GetFileAttributes(pFILEID IN NUMBER) IS
    v_name        FILE_ATTRIBUTES.NAME%TYPE;
    v_datatype    FILE_ATTRIBUTES.DATATYPE%TYPE;
                               
    v_numbervalue FILE_ATTRIBUTES.NUMBERVALUE%TYPE;
    v_datevalue   FILE_ATTRIBUTES.DATEVALUE%TYPE;
    v_stringvalue FILE_ATTRIBUTES.STRINGVALUE%TYPE;
    v_binaryvalue FILE_ATTRIBUTES.BINARYVALUE%TYPE;

    CURSOR c IS
      SELECT fa.NAME, fa.datatype, fa.numbervalue, fa.datevalue, fa.stringvalue, fa.binaryvalue FROM FILE_ATTRIBUTES fa
      WHERE fa.FILEID = pFILEID;
  BEGIN
    OPEN c; 
  
    LOOP        
      FETCH c INTO v_name, v_datatype, v_numbervalue, v_datevalue, v_stringvalue, v_binaryvalue;
      EXIT WHEN c%NOTFOUND;        
      
      DBMS_OUTPUT.put(v_name || CHR(9)); 

      CASE v_datatype
          WHEN 0 THEN DBMS_OUTPUT.put(v_numbervalue);
          WHEN 1 THEN DBMS_OUTPUT.put(v_datevalue);
          WHEN 2 THEN DBMS_OUTPUT.put(v_stringvalue);
          WHEN 3 THEN DBMS_OUTPUT.put(UTL_RAW.CAST_TO_VARCHAR2(v_binaryvalue));
      END CASE;
      DBMS_OUTPUT.new_line;
        
    END LOOP;
	
	  CLOSE c;
    
  END GetFileAttributes;
  
  -- GetFolderAttributes
  Procedure GetFolderAttributes(pFOLDERID IN NUMBER) IS
    v_name        FOLDER_ATTRIBUTES.NAME%TYPE;
    v_datatype    FOLDER_ATTRIBUTES.DATATYPE%TYPE;
                               
    v_numbervalue FOLDER_ATTRIBUTES.NUMBERVALUE%TYPE;
    v_datevalue   FOLDER_ATTRIBUTES.DATEVALUE%TYPE;
    v_stringvalue FOLDER_ATTRIBUTES.STRINGVALUE%TYPE;
    v_binaryvalue FOLDER_ATTRIBUTES.BINARYVALUE%TYPE;

    CURSOR c IS
      SELECT fa.NAME, fa.datatype, fa.numbervalue, fa.datevalue, fa.stringvalue, fa.binaryvalue FROM FOLDER_ATTRIBUTES fa
      WHERE fa.FOLDERID = pFOLDERID;
  BEGIN
    OPEN c; 
  
    LOOP        
      FETCH c INTO v_name, v_datatype, v_numbervalue, v_datevalue, v_stringvalue, v_binaryvalue;
      EXIT WHEN c%NOTFOUND;        
      
      DBMS_OUTPUT.put(v_name || CHR(9)); 

      CASE v_datatype
          WHEN 0 THEN DBMS_OUTPUT.put(v_numbervalue);
          WHEN 1 THEN DBMS_OUTPUT.put(v_datevalue);
          WHEN 2 THEN DBMS_OUTPUT.put(v_stringvalue);
          WHEN 3 THEN DBMS_OUTPUT.put(UTL_RAW.CAST_TO_VARCHAR2(v_binaryvalue));
      END CASE;
      DBMS_OUTPUT.new_line;
        
    END LOOP;
	
	  CLOSE c;
    
  END GetFolderAttributes;
 
  -- GetFiles
  Procedure GetFiles(pFOLDERID IN NUMBER) IS
    v_dt    FILES.DATECREATE%TYPE;
    v_name  FILES.NAME%TYPE;
    v_sz    FILES.SZ%TYPE;
    
    CURSOR c IS
      SELECT f.NAME, f.SZ, f.DATECREATE FROM FILES f WHERE f.folderid = pFOLDERID;
  BEGIN
   OPEN c; 
  
    LOOP        
    
    FETCH c INTO v_name, v_sz, v_dt;
    EXIT WHEN c%NOTFOUND;
        
    DBMS_OUTPUT.put_line(CHR(9) || v_name || CHR(9) || v_sz || CHR(9) || v_dt);
        
    END LOOP;
	
  	CLOSE c;  
  
  END GetFiles;

  -- GetFolders
  Procedure GetFolders(pID IN NUMBER DEFAULT NULL) IS
    v_level NUMBER;
    v_sz    NUMBER;

    v_id    FOLDERS.ID%TYPE;
    v_dt    FOLDERS.DATECREATE%TYPE;
    v_name  FOLDERS.NAME%TYPE;
  
    CURSOR c1 IS 
        SELECT id, SYS_CONNECT_BY_PATH(name, '/'), level, fs.GetFolderSize(ID), datecreate FROM FOLDERS 
        START WITH PARENTID IS NULL
        CONNECT BY PRIOR ID = PARENTID;

    CURSOR c2 IS 
        SELECT id, SYS_CONNECT_BY_PATH(name, '/'), level, fs.GetFolderSize(ID), datecreate FROM FOLDERS 
        START WITH ID = pID
        CONNECT BY PRIOR ID = PARENTID;
  BEGIN
    -- все дерево
    IF pID IS NULL THEN
    
        OPEN c1; 
      
        LOOP        
    
            FETCH c1 INTO v_id, v_name, v_level, v_sz, v_dt;
            EXIT WHEN c1%NOTFOUND;
            
            DBMS_OUTPUT.put_line(v_level || CHR(9) || v_name || CHR(9) || v_sz || CHR(9) || v_dt);
            fs.GetFiles(v_id);
        
        END LOOP;
	
      	CLOSE c1;
    -- только поддерево (начиная с узла)
    ELSE
        OPEN c2; 
      
        LOOP        
    
            FETCH c2 INTO v_id, v_name, v_level, v_sz, v_dt;
            EXIT WHEN c2%NOTFOUND;
        
            DBMS_OUTPUT.put_line(v_level || CHR(9) || v_name || CHR(9) || v_sz || CHR(9) || v_dt);
            fs.GetFiles(v_id);
            
        END LOOP;
	
      	CLOSE c2;
    END IF;

  END GetFolders;
  
  -- UpdateFile  
  Procedure UpdateFile(pID IN NUMBER, pNAME VARCHAR2, pFOLDERID NUMBER, pMAINATTRIBUTES NUMBER DEFAULT 0, pSZ NUMBER DEFAULT 0, pDATA BLOB DEFAULT NULL) IS
  BEGIN
    
    MERGE INTO FILES f
    USING DUAL
    ON (f.ID = pID)
    WHEN MATCHED THEN 
      UPDATE SET NAME = pNAME, FOLDERID = pFOLDERID, MAINATTRIBUTES = pMAINATTRIBUTES, SZ = pSZ, DATA = pDATA
    WHEN NOT MATCHED THEN 
      INSERT (NAME, FOLDERID, MAINATTRIBUTES, SZ, DATA)
      VALUES (pNAME, pFOLDERID, pMAINATTRIBUTES, pSZ, pDATA);
      
    COMMIT;
    
  END UpdateFile;
  
  -- UpdateFolder
  Procedure UpdateFolder(pID IN NUMBER, pNAME VARCHAR2, pMAINATTRIBUTES NUMBER DEFAULT 0, pNESTEDLEVEL NUMBER DEFAULT -1, pPARENTID NUMBER DEFAULT NULL) IS
    cnt NUMBER;
    x   EXCEPTION;
  BEGIN
    
    SELECT COUNT(ID) INTO cnt FROM FOLDERS f WHERE f.id = pID;
    
    IF (cnt != 0) THEN
      UPDATE FOLDERS
      SET NAME = pNAME, MAINATTRIBUTES = pMAINATTRIBUTES, NESTEDLEVEL = pNESTEDLEVEL, PARENTID = pPARENTID, DATEEDIT = CURRENT_DATE
      WHERE ID = pID;
      
    ELSE
      IF (pPARENTID IS NOT NULL) THEN 
        -- проверка всех родителей на превышение ограничения вложенности 
        SELECT COUNT(ID) INTO cnt FROM FOLDERS 
        WHERE (CASE WHEN nestedlevel <> -1 THEN nestedlevel - level -1 ELSE 1 END) <= 0 
        START WITH ID = pPARENTID
        CONNECT BY ID = prior parentID;
        
        IF (cnt > 0) THEN RAISE x; 
        END IF;
      END IF;
    
      INSERT INTO FOLDERS(NAME, MAINATTRIBUTES, NESTEDLEVEL, PARENTID)
      VALUES (pNAME, pMAINATTRIBUTES, pNESTEDLEVEL, pPARENTID);  
    END IF;
    
    COMMIT;
  EXCEPTION
      WHEN x THEN dbms_output.put_line('nestedlevel exceeded'); 
    
  END UpdateFolder;
  
  -- UpdateFileAttribute
  Procedure UpdateFileAttribute(pFILEID IN NUMBER, pNAME VARCHAR2, pDATATYPE IN NUMBER, pNUMBERVALUE IN NUMBER, pDATEVALUE IN DATE, pSTRINGVALUE IN VARCHAR2, pBINARYVALUE IN BLOB) IS   
    x EXCEPTION;
  BEGIN
  
    -- проверка вх. параметров
    CASE pDATATYPE
      WHEN 0 THEN 
        IF pNUMBERVALUE IS NULL THEN RAISE x; 
        END IF;
      WHEN 1 THEN 
        IF pDATEVALUE IS NULL THEN RAISE x; 
        END IF;
      WHEN 2 THEN 
        IF pSTRINGVALUE IS NULL THEN RAISE x; 
        END IF;
      WHEN 3 THEN 
        IF pBINARYVALUE IS NULL THEN RAISE x; 
        END IF;
    END CASE;
  
    MERGE INTO FILE_ATTRIBUTES fa
    USING DUAL
    ON (fa.FILEID = pFILEID AND fa.NAME = pNAME)
    WHEN MATCHED THEN 
      UPDATE SET DATATYPE = pDATATYPE, NUMBERVALUE = pNUMBERVALUE, DATEVALUE = pDATEVALUE, STRINGVALUE = pSTRINGVALUE, BINARYVALUE = pBINARYVALUE
    WHEN NOT MATCHED THEN 
      INSERT (FILEID, NAME, DATATYPE, NUMBERVALUE, DATEVALUE, STRINGVALUE, BINARYVALUE)
      VALUES (pFILEID, pNAME, pDATATYPE, pNUMBERVALUE, pDATEVALUE, pSTRINGVALUE, pBINARYVALUE);
    COMMIT;
    
    EXCEPTION
      WHEN x THEN dbms_output.put_line('attribute value is empty');       
  END UpdateFileAttribute;
  
  -- UpdateFolderAttribute
  Procedure UpdateFolderAttribute(pFOLDERID IN NUMBER, pNAME VARCHAR2, pDATATYPE IN NUMBER, pNUMBERVALUE IN NUMBER, pDATEVALUE IN DATE, pSTRINGVALUE IN VARCHAR2, pBINARYVALUE IN BLOB) IS   
    x EXCEPTION;
  BEGIN
  
    -- проверка вх. параметров
    CASE pDATATYPE
      WHEN 0 THEN 
        IF pNUMBERVALUE IS NULL THEN RAISE x; 
        END IF;
      WHEN 1 THEN 
        IF pDATEVALUE IS NULL THEN RAISE x; 
        END IF;
      WHEN 2 THEN 
        IF pSTRINGVALUE IS NULL THEN RAISE x; 
        END IF;
      WHEN 3 THEN 
        IF pBINARYVALUE IS NULL THEN RAISE x; 
        END IF;
    END CASE;
  
    MERGE INTO FOLDER_ATTRIBUTES fa
    USING DUAL
    ON (fa.FOLDERID = pFOLDERID AND fa.NAME = pNAME)
    WHEN MATCHED THEN 
      UPDATE SET DATATYPE = pDATATYPE, NUMBERVALUE = pNUMBERVALUE, DATEVALUE = pDATEVALUE, STRINGVALUE = pSTRINGVALUE, BINARYVALUE = pBINARYVALUE
    WHEN NOT MATCHED THEN 
      INSERT (FOLDERID, NAME, DATATYPE, NUMBERVALUE, DATEVALUE, STRINGVALUE, BINARYVALUE)
      VALUES (pFOLDERID, pNAME, pDATATYPE, pNUMBERVALUE, pDATEVALUE, pSTRINGVALUE, pBINARYVALUE);
    COMMIT;
    
    EXCEPTION
      WHEN x THEN dbms_output.put_line('attribute value is empty');     
  END UpdateFolderAttribute;
  
end FS;
/

prompt
prompt Creating trigger FILEATTRIBUTE_BEFORE_INSERT
prompt ============================================
prompt
create or replace trigger USER1.fileattribute_before_insert
  before insert on file_attributes  
  for each row
begin
  if :new.id is null then
     select fileattributes_seq.nextval into :new.id from dual;
  end if; 
end fileattribute_before_insert;
/

prompt
prompt Creating trigger FILE_BEFORE_INSERT
prompt ===================================
prompt
create or replace trigger USER1.file_before_insert
  before insert on files  
  for each row
begin
  if :new.id is null then
     select files_seq.nextval into :new.id from dual;
  end if;
end file_before_insert;
/

prompt
prompt Creating trigger FOLDERATTRIBUE_BEFORE_INSERT
prompt =============================================
prompt
create or replace trigger USER1.folderattribue_before_insert
  before insert on folder_attributes  
  for each row
begin
  if :new.id is null then
     select folderattributes_seq.nextval into :new.id from dual;
  end if; 
end folderattribue_before_insert;
/

prompt
prompt Creating trigger FOLDER_BEFORE_INSERT
prompt =====================================
prompt
create or replace trigger USER1.folder_before_insert
  before insert on folders  
  for each row
begin
  if :new.id is null then
     select folders_seq.nextval into :new.id from dual;
  end if;  
end folder_before_insert;
/


spool off
