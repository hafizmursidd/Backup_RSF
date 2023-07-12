delete from SAT_LOCKING where CUSER_ID='hpc'

EXEC RSP_GS_GET_SALES_TAX_PCT_LIST
delete from SAT_LOCKING where CUSER_ID='hpc'
SELECT CBASE_CURRENCY_CODE 
      ,CLOCAL_CURRENCY_CODE
FROM GSM_COMPANY (NOLOCK)
WHERE CCOMPANY_ID = 'Rcd'

SELECT LINCREMENT_FLAG
      ,LAPPROVAL_FLAG 
FROM GSM_TRANSACTION_CODE (NOLOCK)
WHERE CCOMPANY_ID = 'rcd'
AND CTRANSACTION_CODE='000030'

exec RFN_GET_DB_TODAY 'Rcd'

RSP_GL_GET_SYSTEM_PARAM 'Rcd'

SELECT IMIN_YEAR=CAST(MIN(CYEAR) AS INT),IMAX_YEAR=CAST(MAX(CYEAR) AS INT) FROM GSM_PERIOD (NOLOCK)WHERE CCOMPANY_ID = 'RCD'
--getLIST
exec RSP_GL_SEARCH_ACTIVE_REVERSING_LIST 'rcd', 'admin', '202306', '', 'EN'

RSP_GL_GET_JOURNAL_DETAIL_LIST '00', 'EN'


select * from GSM_PERIOD_DT

exec dbo.RFN_GET_DB_TODAY 'RCD'

exec RSP_GL_GET_JOURNAL_DETAIL_LIST '',''

CREC_ID --
LVALID --
CDEPT_CODE --

CDEPT_CODE_NAME --ga ada
CREF_NO
CDOC_NO --gaada
CDOC_DATE --gaada
ISEQUENCE --gaada
CSTART_DATE --gaada
CNEXT_DATE --

CTRANS_DESC
CCURRENCY_CODE
NTRANS_AMOUNT
CLOCAL_CURRENCY_CODE
NLTRANS_AMOUNT
CBASE_CURRENCY_CODE
NBTRANS_AMOUNT

exec [dbo].[RSP_GL_SEARCH_ACTIVE_REVERSING_LIST]
	 'rcd'--@CCOMPANY_ID		VARCHAR(20)
	,'hmc'--@CUSER_ID			VARCHAR(20)
	
	,'202307'--@CPERIOD			CHAR(6)
	,''--@CSEARCH_TEXT		NVARCHAR(30)
	,'en'--@CLANGUAGE_ID		CHAR(2)

	exec RSP_GL_GET_JOURNAL_DETAIL_LIST '2C6AFFF4-848E-405F-9A0C-C6E9AD240748', 'en'

	SELECT LINCREMENT_FLAG
      ,LAPPROVAL_FLAG 
FROM GSM_TRANSACTION_CODE (NOLOCK)
WHERE CCOMPANY_ID = 'RCD'
AND CTRANSACTION_CODE='000030'



	exec RSP_GL_SEARCH_ACTIVE_RECURRING_LIST 'RCD','Admin','202307','','en'

	exec RSP_GL_PROCESS_REVERSING_JRN 'RCD','Admin', '','',''
