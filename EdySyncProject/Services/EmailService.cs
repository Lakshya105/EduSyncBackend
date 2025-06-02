using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;

namespace EdySyncProject.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;
        
        private const string EduSyncLogoUrl = "https://sg1rg1.blob.core.windows.net/image/images.jpg";

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendPasswordResetEmailAsync(string toEmail, string name, string resetLink)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("EduSync", _config["EmailSettings:From"]));
            message.To.Add(new MailboxAddress(name, toEmail));
            message.Subject = "Password Reset - EduSync LMS";

            var htmlBody = $@"
            <table width='100%' cellpadding='0' cellspacing='0' style='background: #f6f8fc; font-family: Arial, sans-serif;'>
                <tr>
                    <td align='center'>
                        <table width='420' cellpadding='0' cellspacing='0' style='background: #fff; border-radius: 16px; box-shadow: 0 8px 24px #e5e9f3; margin-top: 30px;'>
                            <tr>
                                <td align='center' style='padding: 30px 0 10px 0;'>
                                    <img src='{EduSyncLogoUrl}' width='110' alt='EduSync Logo' style='margin-bottom: 15px;'/>
                                </td>
                            </tr>
                            <tr>
                                <td style='padding: 0 36px 10px 36px;'>
                                    <h2 style='color: #3a49a1; margin-bottom: 12px;'>Password Reset Requested</h2>
                                    <p style='color: #444; font-size: 16px;'>Hello <b>{name}</b>,</p>
                                    <p style='color: #555; font-size: 15px; line-height: 1.7;'>
                                        We received a request to reset your password. Click the button below to choose a new password for your EduSync account.
                                    </p>
                                    <div style='text-align: center; margin: 24px 0;'>
                                        <a href='{resetLink}' style='display: inline-block; background: linear-gradient(90deg, #6366f1, #7e22ce); color: #fff; padding: 12px 34px; font-size: 16px; border-radius: 6px; text-decoration: none; box-shadow: 0 2px 8px #ddd;'>
                                            Reset Password
                                        </a>
                                    </div>
                                    <p style='color: #aaa; font-size: 13px;'>
                                        If you didn’t request this, you can safely ignore this email.
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td style='padding: 0 36px 30px 36px; text-align:center; color:#bbb; font-size:12px;'>
                                    &copy; 2025 EduSync LMS &middot; <a href='https://yourplatform.com/privacy' style='color:#7e22ce;'>Privacy Policy</a>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>";

            var builder = new BodyBuilder
            {
                HtmlBody = htmlBody,
                TextBody = $"Hello {name},\n\nWe received a request to reset your password. Click the link below to reset it:\n{resetLink}\n\nIf you did not request this, just ignore this email."
            };

            message.Body = builder.ToMessageBody();

            try
            {
                using var client = new SmtpClient();
                await client.ConnectAsync(_config["EmailSettings:SmtpServer"], int.Parse(_config["EmailSettings:Port"]), true);
                await client.AuthenticateAsync(_config["EmailSettings:Username"], _config["EmailSettings:Password"]);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                Console.WriteLine("Password reset email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send reset email: " + ex.Message);
                throw;
            }
        }

        public async Task SendWelcomeEmailAsync(string toEmail, string name)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("EduSync", _config["EmailSettings:From"]));
            message.To.Add(new MailboxAddress(name, toEmail));
            message.Subject = "Welcome to EduSync LMS!";

            var htmlBody = $@"
            <table width='100%' cellpadding='0' cellspacing='0' style='background: #f6f8fc; font-family: Arial, sans-serif;'>
                <tr>
                    <td align='center'>
                        <table width='420' cellpadding='0' cellspacing='0' style='background: #fff; border-radius: 16px; box-shadow: 0 8px 24px #e5e9f3; margin-top: 30px;'>
                            <tr>
                                <td align='center' style='padding: 32px 0 18px 0;'>
                                    <img src='{EduSyncLogoUrl}' width='110' alt='EduSync Logo' style='margin-bottom: 15px;'/>
                                </td>
                            </tr>
                            <tr>
                                <td style='padding: 0 36px 10px 36px;'>
                                    <h2 style='color: #3a49a1; margin-bottom: 14px;'>Welcome to EduSync, {name}!</h2>
                                    <p style='color: #444; font-size: 16px;'>
                                        We’re excited to have you join our learning community.
                                    </p>
                                    <p style='color: #555; font-size: 15px; line-height: 1.7;'>
                                        Explore new courses, take interactive assessments, and track your progress with EduSync LMS.<br>
                                        Click below to get started:
                                    </p>
                                    <div style='text-align: center; margin: 24px 0;'>
                                        <a href='https://edusync-g2f8btagfjang3gb.centralindia-01.azurewebsites.net'
                                           style='display: inline-block; background: linear-gradient(90deg, #6366f1, #7e22ce); color: #fff; padding: 12px 34px; font-size: 16px; border-radius: 6px; text-decoration: none; box-shadow: 0 2px 8px #ddd;'>
                                            Login Now
                                        </a>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style='padding: 0 36px 30px 36px; text-align:center; color:#bbb; font-size:12px;'>
                                    &copy; 2025 EduSync LMS &middot; <a href='https://yourplatform.com/privacy' style='color:#7e22ce;'>Privacy Policy</a>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>";

            var builder = new BodyBuilder
            {
                HtmlBody = htmlBody,
                TextBody = $"Hello {name},\n\nWelcome to EduSync LMS! Start exploring your courses and assessments.\n\nBest Regards,\nThe EduSync Team"
            };

            message.Body = builder.ToMessageBody();

            try
            {
                using var client = new SmtpClient();
                await client.ConnectAsync(_config["EmailSettings:SmtpServer"], int.Parse(_config["EmailSettings:Port"]), true);
                await client.AuthenticateAsync(_config["EmailSettings:Username"], _config["EmailSettings:Password"]);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                Console.WriteLine("Welcome email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send welcome email: " + ex.Message);
            }
        }
    }
}
