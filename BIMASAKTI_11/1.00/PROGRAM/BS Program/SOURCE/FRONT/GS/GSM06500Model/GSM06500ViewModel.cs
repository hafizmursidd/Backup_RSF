﻿using GSM06500Common;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace GSM06500Model
{
    public class GSM06500ViewModel : R_ViewModel<GSM06500DTO>
    {
        private GSM06500Model _model = new GSM06500Model();
        public ObservableCollection<GSM06500DTO> TermofPaymentList = new ObservableCollection<GSM06500DTO>();

        public string lcPropertyId { get; set; }

        public async Task GetAllTermOfPaymentAsync()
        {
            R_Exception loException = new R_Exception();
            try
            {
                var loResult = await _model.GetTermOfPaymentListAsyncModel();
                TermofPaymentList = new ObservableCollection<GSM06500DTO>(loResult.ListData);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

        EndBlock:
            loException.ThrowExceptionIfErrors();
        }

        public async Task DeleteTermOfPayment(GSM06500DTO poProperty)
        {
            var loEx = new R_Exception();

            try
            {
                var loParam = new GSM06500DTO
                {
                    CCOMPANY_ID = poProperty.CCOMPANY_ID,
                    CPROPERTY_ID = poProperty.CPROPERTY_ID,
                    CUSER_ID = poProperty.CUSER_ID,
                    CPAY_TERM_CODE = poProperty.CPAY_TERM_CODE,
                    CPAY_TERM_NAME = poProperty.CPAY_TERM_NAME,
                    IPAY_TERM_DAYS = poProperty.IPAY_TERM_DAYS
                };
                await _model.R_ServiceDeleteAsync(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task<GSM06500DTO> GetTermOftermOneRecord(GSM06500DTO poProperty)
        {
            var loEx = new R_Exception();
            GSM06500DTO loResult = null;

            try
            {
                var loParam = new GSM06500DTO
                {
                    CPROPERTY_ID = poProperty.CPROPERTY_ID,
                    CPAY_TERM_CODE = poProperty.CPAY_TERM_CODE
                };
                loResult = await _model.R_ServiceGetRecordAsync(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }

        public async Task<GSM06500DTO> SaveProduct(GSM06500DTO poNewEntity, R_eConductorMode peConductorMode)
        {
            var loEx = new R_Exception();
            GSM06500DTO loResult = null;

            try
            {

                loResult = await _model.R_ServiceSaveAsync(poNewEntity, (eCRUDMode)peConductorMode);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }

    }
}
