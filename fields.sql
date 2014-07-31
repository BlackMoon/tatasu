DECLARE
   ix   number(2);
   n    number := 10;
   
   minv number := 0;
   maxv number := 1000;
   
   v_field1 data.field_1%TYPE;
   v_field2 data.field_2%TYPE;
   v_field3 data.field_3%TYPE;
   v_field4 data.field_4%TYPE;
   v_field5 data.field_5%TYPE;
   v_field6 data.field_6%TYPE;
   
   CURSOR data_cursor IS 
          SELECT * FROM data WHERE 
                 ((CASE WHEN field_1 > 0 THEN 1 ELSE 0 END) + 
                 (CASE WHEN field_2 > field_3 THEN 1 ELSE 0 END) + 
                 (CASE WHEN field_5 < field_4 THEN 1 ELSE 0 END) +
                 (CASE WHEN field_2 > field_6 THEN 1 ELSE 0 END) +
                 (CASE WHEN field_2 = field_5 THEN 1 ELSE 0 END) +
                 (CASE WHEN field_3 > 78 THEN 1 ELSE 0 END) +
                 (CASE WHEN field_6 = 0 THEN 1 ELSE 0 END)) >= 4;
BEGIN
  
  -- fill rows  
  DELETE FROM data;
 
  FOR ix in 1 .. n LOOP
    INSERT INTO data(field_1, field_2, field_3, field_4, field_5, field_6)
    VALUES(
           dbms_random.value(minv, maxv), 
           dbms_random.value(minv, maxv), 
           dbms_random.value(minv, maxv), 
           dbms_random.value(minv, maxv), 
           dbms_random.value(minv, maxv), 
           dbms_random.value(minv, maxv)
           );     
  END LOOP;  
  COMMIT;

  -- get rows  
  DBMS_OUTPUT.enable;
  ix := 0;
  
  OPEN data_cursor; 
  
  LOOP        
    ix := ix + 1;
    FETCH data_cursor INTO v_field1, v_field2, v_field3, v_field4, v_field5, v_field6;
    EXIT WHEN data_cursor%NOTFOUND;
        
    DBMS_OUTPUT.put_line(ix || CHR(9) || CHR(9) || v_field1 || CHR(9) || v_field2 || CHR(9) || v_field3 || CHR(9) || v_field4 || CHR(9) || v_field5 || CHR(9) || v_field6);
        
  END LOOP;
	
	CLOSE data_cursor;
END;
