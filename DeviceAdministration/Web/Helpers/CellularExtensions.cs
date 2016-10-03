﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviceManagement.Infrustructure.Connectivity.Models.Constants;
using DeviceManagement.Infrustructure.Connectivity.Models.Other;
using DeviceManagement.Infrustructure.Connectivity.Models.TerminalDevice;
using DeviceManagement.Infrustructure.Connectivity.Services;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.Models;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Infrastructure.Repository;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Web.Models;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Web.Helpers
{
    public class CellularExtensions : ICellularExtensions
    {
        private readonly IExternalCellularService _cellularService;
        private readonly IIccidRepository _iccidRepository;

        public CellularExtensions(IExternalCellularService cellularService, IIccidRepository iccidRepository)
        {
            if (cellularService == null)
            {
               throw new ArgumentNullException(nameof(cellularService));
            }
            _cellularService = cellularService;
            _iccidRepository = iccidRepository;
        }

        public List<Iccid> GetTerminals()
        {
            return this._cellularService.GetTerminals();
        }

        public Terminal GetSingleTerminalDetails(Iccid iccid)
        {
            return this._cellularService.GetSingleTerminalDetails(iccid);
        }

        public List<SessionInfo> GetSingleSessionInfo(Iccid iccid)
        {
            return this._cellularService.GetSingleSessionInfo(iccid);
        }

        public IEnumerable<string> GetListOfAvailableIccids(IList<DeviceModel> devices, string providerName)
        {
            var fullIccidList = providerName == ApiRegistrationProviderTypes.Ericsson ? 
                _iccidRepository.GetIccids().Select(e => e.Id) : 
                _cellularService.GetTerminals().Select(i => i.Id);

            var usedIccidList = GetUsedIccidList(devices).Select(i => i.Id);
            return fullIccidList.Except(usedIccidList);
        }

        public IEnumerable<string> GetListOfAvailableDeviceIDs(IList<DeviceModel> devices)
        {
            return (from device in devices
                    where (device.DeviceProperties != null && device.DeviceProperties.DeviceID != null) &&
                          (device.SystemProperties == null || device.SystemProperties.ICCID == null)
                    select device.DeviceProperties.DeviceID
                   ).Cast<string>().ToList();
        }

        public IEnumerable<string> GetListOfConnectedDeviceIds(IList<DeviceModel> devices)
        {
            return (from device in devices
                    where (device.DeviceProperties != null && device.DeviceProperties.DeviceID != null) &&
                        (device.SystemProperties == null || device.SystemProperties.ICCID != null)
                    select device.DeviceProperties.DeviceID
                    ).ToList();
        }

        public bool ValidateCredentials()
        {
            return _cellularService.ValidateCredentials();
        }

        public List<SimState> GetAllAvailableSimStates(string iccid, string currentState)
        {
            var availableSimStates = _cellularService.GetValidTargetSimStates(new SimState() {IsActive = true, Name = currentState });
            return availableSimStates;
        }

        public List<SimState> GetValidTargetSimStates(string currentState)
        {
            var availableSimStates = _cellularService.GetValidTargetSimStates(new SimState() { IsActive = true, Name = currentState });
            return availableSimStates;
        }

        public List<SubscriptionPackage> GetAvailableSubscriptionPackages(string currentSubscriptionPackageName)
        {
            var availableSubscriptions = _cellularService.GetAvailableSubscriptionPackages();
            return markActiveSubscriptionPackage(currentSubscriptionPackageName, availableSubscriptions);
        }

        public async Task<bool> UpdateSimState(string iccid, string updatedState)
        {
            return await _cellularService.UpdateSimState(iccid, updatedState);
        }

        public async Task<bool> UpdateSubscriptionPackage(string iccid, string updatedPackage)
        {
            return await _cellularService.UpdateSubscriptionPackage(iccid, updatedPackage);
        }

        public async Task<bool> ReconnectDevice(string iccid)
        {
            return await _cellularService.ReconnectTerminal(iccid);
        }

        public async Task<bool> SendSms(string iccid, string smsText)
        {
            return await _cellularService.SendSms(iccid, smsText);
        }

        private IEnumerable<Iccid> GetUsedIccidList(IList<DeviceModel> devices)
        {
            return (from device in devices
                    where device.DeviceProperties?.DeviceID != null && 
                        device.SystemProperties?.ICCID != null
                    select new Iccid(device.SystemProperties.ICCID)
                   ).ToList();
        }

        private List<SubscriptionPackage> markActiveSubscriptionPackage(string selectedSubscriptionName, List<SubscriptionPackage> availableSubscriptionPackages)
        {
            return availableSubscriptionPackages.Select(s =>
            {
                if (s.Name == selectedSubscriptionName) s.IsActive = true;
                return s;
            }).ToList();
        }

        private List<SimState> markActiveSimState(string selectedSubscriptionName, List<SimState> availableSimStates)
        {
            return availableSimStates.Select(s =>
            {
                if (s.Name == selectedSubscriptionName) s.IsActive = true;
                return s;
            }).ToList();
        }
    }
}