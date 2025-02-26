﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tracker.Data.DTO;
using Tracker.Data.Models;
using Tracker.Domain.IRepositories;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Tracker.Core.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<EmployeeDTO> GetAll()
        {
            var emps = _context.Employees.Select(e => new EmployeeDTO
            {
                Id = e.Id,
                EmployeeName = e.EmployeeName,
                DepartmentName = e.Department.Name,
                Position = e.Position,
                Address = e.Address,
                EmployeeCode = e.EmployeeCode,
                // DateOfBirth = e.DateOfBirth,
                Email = e.Email,
                gender = e.gender,
                //HiringDateHiringDate = e.HiringDateHiringDate,
                MaritalStatus = e.MaritalStatus,
                Phone = e.Phone,
                Mobile = e.Mobile,
                Photo = e.Photo
            }).ToList();
            return emps;
        }
        public EmployeeDTO GetById(int id)
        {
            var e = _context.Employees.Include(e => e.Department).FirstOrDefault(e => e.Id == id);
            var emp = new EmployeeDTO
            {
                Id = e.Id,
                EmployeeName = e.EmployeeName,
                DepartmentName = e.Department.Name,
                DepartmentId = e.DepartmentId,
                Address = e.Address,
                Position = e.Position,
                EmployeeCode = e.EmployeeCode,
                Email = e.Email,
                gender = e.gender,
                MaritalStatus = e.MaritalStatus,
                Phone = e.Phone,
                Mobile = e.Mobile,
                Photo = e.Photo
            };
            if (emp == null)
            {
                return null;
            }

            return emp;
        }
        public void Add(EmployeeDTO employeeDTO)
        {
             if (employeeDTO != null)
                {
                    var codeFound = _context.Employees.Where(e => e.EmployeeCode == employeeDTO.EmployeeCode && e.Id != employeeDTO.Id).ToList();
                    if (codeFound.Count > 0)
                    {
                        throw new AlreadyFoundException("code already found");
                    }
                    var emailFound = _context.Employees.Where(e => e.Email == employeeDTO.Email && e.Id != employeeDTO.Id).ToList();
                      if(emailFound.Count >0)
                        {
                        throw new AlreadyFoundException("email already found");
                        }
                    var phonefound = _context.Employees.Where(e => e.Phone == employeeDTO.Phone && e.Id != employeeDTO.Id).ToList();
                    if(phonefound.Count>0)
                     {
                       throw new AlreadyFoundException("phone already found");
                     }
                    
                    else
                    {
                        var emp = new Employee();
                        emp.EmployeeName = employeeDTO.EmployeeName;
                        emp.Position = employeeDTO.Position;
                        emp.DepartmentId = employeeDTO.DepartmentId;
                        emp.EmployeeCode = employeeDTO.EmployeeCode;
                        emp.gender = employeeDTO.gender;
                        emp.Address = employeeDTO.Address;
                        emp.MaritalStatus = employeeDTO.MaritalStatus;
                        emp.Phone = employeeDTO.Phone;
                        emp.Mobile = employeeDTO.Mobile;
                        emp.Email = employeeDTO.Email;
                        emp.Photo = employeeDTO.Photo;
                        _context.Employees.Add(emp);

                

                    _context.SaveChanges();
                    }
                     
                    
                }
                else
                {
                    throw new NotCompletedException("Not Completed Exception");
                }
            }
        //catch (Exception)
        //{
        //    throw new NotExistException("Not Exist Exception");
        //}

        /*
         Client client = _context.clients.Find(id);
           if (client != null)
           {
               _context.clients.Remove(client);
               _context.SaveChanges();
           }
           else
           {
               throw new NotExistException("Not Exist Exception");
           }
         */
        public void Delete(int id)
        {
            Employee employee = _context.Employees.Find(id);
            if (employee != null)
            {
                var projectTeam = _context.projectTeams.Where(e => e.EmployeeId == id).ToList();
                if (projectTeam.Count > 0)
                {
                    for (int i = 0; i < projectTeam.Count; i++)
                    {
                        _context.projectTeams.Remove(projectTeam[i]);
                      //  _context.SaveChanges();
                    }
                }
                _context.Employees.Remove(employee);
                _context.SaveChanges();
            }
            else
            {
                throw new NotExistException("Not Exist Exception");
            }
        }
        public Employee Find(int id)
        {
            return _context.Employees.Find(id);
        }
        public void Update(int id, EmployeeDTO employeeDTO)
        {
            if (id != employeeDTO.Id)
            {
                throw new NotExistException("Not Exist Exception");
            }
            var codeFound = _context.Employees.Where(e => e.EmployeeCode == employeeDTO.EmployeeCode && e.Id != employeeDTO.Id).ToList();
            if (codeFound.Count > 0)
            {
                throw new AlreadyFoundException("code already found");
            }
            var emailFound = _context.Employees.Where(e => e.Email == employeeDTO.Email && e.Id != employeeDTO.Id).ToList();
            if (emailFound.Count > 0)
            {
                throw new AlreadyFoundException("email already found");
            }
            var phonefound = _context.Employees.Where(e => e.Phone == employeeDTO.Phone && e.Id != employeeDTO.Id).ToList();
            if (phonefound.Count > 0)
            {
                throw new AlreadyFoundException("phone already found");
            }
            else
            {
                var emp = new Employee();
                emp.Id = employeeDTO.Id;
                emp.EmployeeName = employeeDTO.EmployeeName;
                emp.Position = employeeDTO.Position;
                emp.DepartmentId = employeeDTO.DepartmentId;
                emp.EmployeeCode = employeeDTO.EmployeeCode;
                emp.gender = employeeDTO.gender;
                emp.Address = employeeDTO.Address;
                emp.MaritalStatus = employeeDTO.MaritalStatus;
                emp.Phone = employeeDTO.Phone;
                emp.Mobile = employeeDTO.Mobile;
                emp.Email = employeeDTO.Email;
                emp.Photo = employeeDTO.Photo;
                _context.Entry(emp).State = EntityState.Modified;
                _context.SaveChanges();
            }
            //try
            //{
           
            //}
            //catch (Exception)
            //{
            //    throw new NotCompletedException("Not Completed Exception");
            //}
        }

        public void Save()
        {
            _context.SaveChanges();
        }
        public List<EmployeeDTO> GetEmployeeByDepartmentId(int departmentId)
        {
            var e = _context.Employees.Where(e => e.DepartmentId == departmentId).Select(e => new EmployeeDTO
            {

                Id = e.Id,
                EmployeeName = e.EmployeeName,
                DepartmentName = e.Department.Name,
                DepartmentId = e.DepartmentId,
                Address = e.Address,
                Position = e.Position,
                EmployeeCode = e.EmployeeCode,
                Email = e.Email,
                gender = e.gender,
                MaritalStatus = e.MaritalStatus,
                Phone = e.Phone,
                Mobile = e.Mobile,
                Photo = e.Photo
            }).ToList();
            if (e == null)
            {
                return null;
            }

            return e;
        }

        public void DeleteEmployeeAndProjectTeam(int EmployeeId)
        {
            throw new NotImplementedException();
        }
    }
}
