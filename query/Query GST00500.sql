use BIMASAKTI_11

--Password: CD-r5fdbsvr1#
delete from SAT_LOCKING where CUSER_ID='aoc'
rollback
select * from SAT_LOCKING


SELECT VAR_SELECTED = 0, A.*, C.CDESCRIPTION AS CAPPROVAL_STATUS_DESC, B.CTRANSACTION_NAME, B.CTABLE_NAME, B.CPROGRAM_ID 
FROM GST_APPROVAL_I A (NOLOCK) 
INNER JOIN GSM_TRANSACTION_CODE B (NOLOCK) ON B.CCOMPANY_ID = A.CCOMPANY_ID AND B.CTRANSACTION_CODE = A.CTRANSACTION_CODE 
INNER JOIN RFT_GET_GSB_CODE_INFO('SIAPP', [LOGIN USER].[CCOPANY_ID], '_GS_APPROVAL_STATUS', '', [LOGIN USER].[CLANGUAGE_ID]) C ON C.CCODE = A.CAPPROVAL_STATUS
WHERE A.CUSER_ID = [LOGIN USER ID] AND A.CAPPROVAL_STATUS = '01'  AND A.CTRANSACTION_STATUS IN ('01','02')
ORDER BY B.CTRANSACTION_NAME, A.CREFERENCE_DATE ASC

SELECT *FROM GST_APPROVAL_I 


select * from GSM_TRANSACTION_CODE
select * from GST_APPROVAL_I
delete from  GST_APPROVAL_I where CREFERENCE_NO = 'REF.202306.0003'
--�	Combobox. �Reason of Rejection�
SELECT * FROM RFT_GET_GSB_CODE_INFO('SIAPP', 'rcd', '_GS_REJECTTRANS_REASON',' ','EN')
SELECT * FROM RFT_GET_GSB_CODE_INFO('SIAPP', 'RCD', '_GS_REJECTTRANS_REASON', '' , 'EN') 

Select CCOMPANY_ID From GST_APPROVAL_I (Updlock) 
Where CCOMPANY_ID = 'rcd' And CTRANSACTION_CODE = '000000' And CDEPT_CODE = 'ACC' And CREFERENCE_NO = 'REF.202306.0001' And CTRANSACTION_STATUS In (01,02); 
Update GST_APPROVAL_I Set CAPPROVAL_STATUS= '01' FROM GST_APPROVAL_I A (NOLOCK)

select * from GST_APPROVAL_I (updlock) 
select * FROM GST_APPROVAL_I 
select 
CCODE	CDESCRIPTION
01                  	Invalid Data Entry

WITH Inbox.CurrentSelectedRecord:
Set Var_Company_Id = 'RCD',
Set Var_Transaction_Code = .CTRANSACTION_CODE
Set Var_Department_Code = .CDEPT_CODE
Set Var_Reference_No = .CREFERENCE_NO
Run program .CPROGRAM_ID With Parameters: Var_Company_Id And Var_Transaction_Code And Var_Department_Code And Var_Reference_No + Access View Only
End WITH

SELECT VAR_SELECTED = 1, A.*, C.CDESCRIPTION AS CTRANSACTION_STATUS_DESC, B.CTRANSACTION_NAME, B.CPROGRAM_ID FROM GST_APPROVAL_O A (NOLOCK) 
INNER JOIN GSM_TRANSACTION_CODE B (NOLOCK) ON B.CCOMPANY_ID = A.CCOMPANY_ID AND B.CTRANSACTION_CODE = A.CTRANSACTION_CODE 
INNER JOIN DBO.RFT_GET_GSB_CODE_INFO('SIAPP', 'RCD', '_STATUS_FLAG_01', '','EN') C ON C.CCODE = A.CTRANSACTION_STATUS
WHERE A.CUSER_ID = 'HPC' AND A.CTRANSACTION_STATUS IN ('01','02')
ORDER BY B.CTRANSACTION_NAME, A.CREFERENCE_DATE ASC



SELECT B.CUSER_NAME FROM SAM_USER_COMPANY A (NOLOCK) 
INNER JOIN SAM_USER B (NOLOCK) ON B.CUSER_ID = A.CUSER_ID 
WHERE A.CCOMPANY_ID = 'Rcd' AND A.CUSER_ID = 'IB'

select * from SAM_USER_COMPANY
select * from SAM_USER




SELECT VAR_SELECTED = 1, A.*, C.CDESCRIPTION AS CTRANSACTION_STATUS_DESC, B.CTRANSACTION_NAME, B.CPROGRAM_ID FROM GST_APPROVAL_O A (NOLOCK) 
INNER JOIN GSM_TRANSACTION_CODE B (NOLOCK) ON B.CCOMPANY_ID = A.CCOMPANY_ID AND B.CTRANSACTION_CODE = A.CTRANSACTION_CODE 
INNER JOIN DBO.RFT_GET_GSB_CODE_INFO('SIAPP', 'rcd', '_STATUS_FLAG_01', '', 'en') C ON C.CCODE = A.CTRANSACTION_STATUS
WHERE A.CUSER_ID = 'HPC' AND A.CTRANSACTION_STATUS IN ('01','02')
ORDER BY B.CTRANSACTION_NAME, A.CREFERENCE_DATE ASC

-------------------------INBOX-----------------------------

SELECT VAR_SELECTED = 0, A.*--, C.CDESCRIPTION AS CAPPROVAL_STATUS_DESC, B.CTRANSACTION_NAME, B.CTABLE_NAME, B.CPROGRAM_ID 
FROM GST_APPROVAL_I A (NOLOCK) 
INNER JOIN GSM_TRANSACTION_CODE B (NOLOCK) ON B.CCOMPANY_ID = A.CCOMPANY_ID AND B.CTRANSACTION_CODE = A.CTRANSACTION_CODE 
INNER JOIN RFT_GET_GSB_CODE_INFO('SIAPP', 'RCD', '_GS_APPROVAL_STATUS', '', 'EN') C ON C.CCODE = A.CAPPROVAL_STATUS
WHERE A.CUSER_ID = 'HPC' AND A.CAPPROVAL_STATUS = '01'  AND A.CTRANSACTION_STATUS IN ('01','02')
ORDER BY B.CTRANSACTION_NAME, A.CREFERENCE_DATE ASC


SELECT VAR_SELECTED = 0, A.*, C.CDESCRIPTION AS CAPPROVAL_STATUS_DESC, B.CTRANSACTION_NAME,
                              B.CTABLE_NAME, B.CPROGRAM_ID FROM GST_APPROVAL_I A (NOLOCK) 
                              INNER JOIN GSM_TRANSACTION_CODE B (NOLOCK) 
                              ON B.CCOMPANY_ID = A.CCOMPANY_ID AND B.CTRANSACTION_CODE = A.CTRANSACTION_CODE 
                              INNER JOIN RFT_GET_GSB_CODE_INFO('SIAPP', 'Rcd',
                              '_GS_APPROVAL_STATUS', '', 'en') C 
                              ON C.CCODE = A.CAPPROVAL_STATUS 
                              WHERE A.CUSER_ID = 'hpc' AND A.CAPPROVAL_STATUS = '01'  AND A.CTRANSACTION_STATUS IN ('01','02') 
                              ORDER BY B.CTRANSACTION_NAME, A.CREFERENCE_DATE ASC
select * from GST_APPROVAL_I
select * from GSM_TRANSACTION_CODE
select * from RFT_GET_GSB_CODE_INFO('SIAPP', 'Rcd',
                              '_GS_APPROVAL_STATUS', '', 'en')

Select CCOMPANY_ID From GST_APPROVAL_I (Updlock) 
Where CCOMPANY_ID = 'rcd' And CTRANSACTION_CODE = '000010' And CDEPT_CODE = 'ACC' And CREFERENCE_NO = 'REF.202306.0001' And CTRANSACTION_STATUS In (01,02); 
Update GST_APPROVAL_I Set CAPPROVAL_STATUS= '01' FROM GST_APPROVAL_I A (NOLOCK)

--value 2
Select CCOMPANY_ID From GST_APPROVAL_I (Updlock) 
Where CCOMPANY_ID = 'rcd' And CTRANSACTION_CODE = '000010' And CDEPT_CODE = 'ACC' And CREFERENCE_NO = 'REF.202306.0003' And CTRANSACTION_STATUS In (00,02); 
Update GST_APPROVAL_I Set CAPPROVAL_STATUS= '01' FROM GST_APPROVAL_I A (NOLOCK)

select * from GST_APPROVAL_I 
insert into GST_APPROVAL_I 
(CCOMPANY_ID, CTRANSACTION_CODE, CDEPT_CODE,CREFERENCE_NO ,CUSER_ID, CAPPROVAL_USER_ID, CPERIOD, ISEQUENCE, CREFERENCE_DATE,  CAPPROVAL_STATUS, CTRANSACTION_STATUS)
Values 
('rcd', '000010', 'ACC', 'REF.202306.0004', 'HPC', 'HPC', '202306', 1, '20230620', '01', '01')

Select CCOMPANY_ID From GST_APPROVAL_I  (Updlock) Where CCOMPANY_ID = 'rcd' And CTRANSACTION_CODE = 'Normal Journal' And CDEPT_CODE = 'ACC'And CREFERENCE_NO = 'REF.202306.0001' And CTRANSACTION_STATUS In (01,02) 
UPDATE GST_APPROVAL_I SET CAPPROVAL_STATUS= '02' 
FROM GST_APPROVAL_I A (NOLOCK) WHERE A.CCOMPANY_ID= 'rcd' AND A.CTRANSACTION_CODE = '000000' AND A.CDEPT_CODE= 'ACC' AND A.CREFERENCE_NO= 'REF.202306.0001' AND A.CUSER_ID = 'hmc' AND A.CAPPROVAL_STATUS= '01'

rollback;

SELECT * FROM RFT_GET_GSB_CODE_INFO('SIAPP', 'RCD', '_GS_REJECTTRANS_REASON', '' , 'EN') 

UPDATE GST_APPROVAL_I 
SET CAPPROVAL_STATUS	= '01'
FROM GST_APPROVAL_I A (NOLOCK) 
WHERE A.CCOMPANY_ID	= 'RCD'
AND A.CTRANSACTION_CODE 	= '000000'
AND A.CDEPT_CODE		= 'ACC'
AND A.CREFERENCE_NO	= 'REF.202306.0001'
AND A.CUSER_ID		= 'HPC'
AND A.CAPPROVAL_STATUS	= '02'

select *
FROM GST_APPROVAL_I A (NOLOCK) 
WHERE A.CCOMPANY_ID	= 'RCD'
AND A.CTRANSACTION_CODE 	= '000000'
AND A.CDEPT_CODE		= 'ACC'
AND A.CREFERENCE_NO	= 'REF.202306.0001'
AND A.CUSER_ID		= 'HPC'
AND A.CAPPROVAL_STATUS	= '02'


-------------------------OUTBOX-----------------------------

   SELECT VAR_SELECTED = 1, A.*, C.CDESCRIPTION AS CTRANSACTION_STATUS_DESC, B.CTRANSACTION_NAME,
                   B.CPROGRAM_ID FROM GST_APPROVAL_O A(NOLOCK) 
                   INNER JOIN GSM_TRANSACTION_CODE B(NOLOCK) ON B.CCOMPANY_ID = A.CCOMPANY_ID 
                   AND B.CTRANSACTION_CODE = A.CTRANSACTION_CODE 
                    INNER JOIN DBO.RFT_GET_GSB_CODE_INFO('SIAPP', 'rcd', 
                    '_STATUS_FLAG_01', '', 'en') C 
                    ON C.CCODE = A.CTRANSACTION_STATUS
                   WHERE A.CUSER_ID = 'hpc' AND A.CTRANSACTION_STATUS IN('01','02') 
                   ORDER BY B.CTRANSACTION_NAME, A.CREFERENCE_DATE ASC

				    -----------------------OUTBOX list of Approve by � Outbox

 SELECT C.*, D.CUSER_NAME, D.CPOSITION, E.CDESCRIPTION AS CAPPROVAL_STATUS_DESC FROM GST_APPROVAL_I C (NOLOCK) 
INNER JOIN SAM_USER D (NOLOCK) ON D.CUSER_ID = C.CUSER_ID 
INNER JOIN RFT_GET_GSB_CODE_INFO ('SIAPP', 'RCD', '_GS_APPROVAL_STATUS', '', 'EN') E ON C.CAPPROVAL_STATUS = E.CCODE
WHERE C.CCOMPANY_ID     = 'RCD'
AND C.CTRANSACTION_CODE = '000000'
AND C.CDEPT_CODE        = 'ACC'
AND C.CREFERENCE_NO     = 'REF.202306.0001'
ORDER BY C.CUSER_ID, C.ISEQUENCE ASC

SELECT * FROM RFT_GET_GSB_CODE_INFO(�SIAPP�, [Login User].[CCOMPANY_ID], �_GS_REJECTTRANS_REASON�, ��, [Login User].[CLANGUAGE_ID])
				   -------------------------DRAFT-----------------------------

SELECT VAR_SELECTED = 1, A.*, B.CTRANSACTION_NAME, B.CPROGRAM_ID FROM GST_APPROVAL_O A(NOLOCK) 
INNER JOIN GSM_TRANSACTION_CODE B(NOLOCK) ON B.CCOMPANY_ID = A.CCOMPANY_ID
AND B.CTRANSACTION_CODE = A.CTRANSACTION_CODE 
WHERE A.CUSER_ID = 'HPC' AND A.CTRANSACTION_STATUS = '00'

UPDATE GST_APPROVAL_I 
SET CAPPROVAL_STATUS	= '02'
FROM GST_APPROVAL_I A (NOLOCK) 
WHERE A.CCOMPANY_ID	= 'RCD'
AND A.CTRANSACTION_CODE 	= '000000'
AND A.CDEPT_CODE		= 'ACC'
AND A.CREFERENCE_NO	= 'REF.202306.0001'
AND A.CUSER_ID		= 'HPC'
AND A.CAPPROVAL_STATUS	= '01'
-----------------LOCKING
 Select CCOMPANY_ID From GST_APPROVAL_I  (Updlock)
Where CCOMPANY_ID = '{poParameter.CCOMPANYID}' And CTRANSACTION_CODE = '{item.CTRANSACTION_NAME}' 
 And CDEPT_CODE = '{item.CDEPT_CODE}'And CREFERENCE_NO = '{item.CREFERENCE_NO}' And CTRANSACTION_STATUS In (01,02)

 select CCOMPANY_ID from GST_APPROVAL_I
 
 exec RSP_LM_GET_CHARGES_TYPE_LIST 'rcd','jbmpc', '02', 'hmc'